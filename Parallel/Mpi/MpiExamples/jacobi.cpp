#include <mpi.h>
#include <omp.h>
#include <algorithm>
#include <cmath>
#include <cstdlib>
#include <numbers>
#include <print>
#include <vector>

// Метод Якобі для системи (2 + σ)·u[i] - u[i-1] - u[i+1] = b[i],
// i = 0…n-1, u[-1] = u[n] = 0 (крок неявної схеми теплопровідності).
// Праву частину b обрано так, що точний розв'язок u[i] = sin(πx[i]).
// Аргументи: кількість невідомих n, кількість ітерацій.
const double Sigma = 0.1;

double Exact(long g, long n)             // g – глобальний номер вузла
{
    return std::sin(std::numbers::pi * (g + 1) / (n + 1));
}

int main(int argc, char* argv[])
{
    int provided = 0;
    MPI_Init_thread(&argc, &argv, MPI_THREAD_FUNNELED, &provided);
    int rank = 0, size = 0;
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);
    if (provided < MPI_THREAD_FUNNELED)
        MPI_Abort(MPI_COMM_WORLD, 1);

    const long n = argc > 1 ? std::atol(argv[1]) : 8'000'000;
    const int iters = argc > 2 ? std::atoi(argv[2]) : 500;
    // Блоковий розподіл: перші n % size рангів отримують на 1 більше.
    const long local = n / size + (rank < n % size ? 1 : 0);
    const long first = rank * (n / size)
                       + std::min<long>(rank, n % size);
    // Сусіди; MPI_PROC_NULL – «немає сусіда», обмін з ним порожній.
    const int left = rank > 0 ? rank - 1 : MPI_PROC_NULL;
    const int right = rank < size - 1 ? rank + 1 : MPI_PROC_NULL;

    // u[0] і u[local + 1] – гало-комірки з вузлами сусідів.
    std::vector<double> u(local + 2, 0.0), v(local + 2, 0.0),
                        b(local + 2);
    #pragma omp parallel for
    for (long i = 1; i <= local; ++i)
    {
        long g = first + i - 1;
        b[i] = (2 + Sigma) * Exact(g, n)
               - (g > 0 ? Exact(g - 1, n) : 0)
               - (g < n - 1 ? Exact(g + 1, n) : 0);
    }

    double commTime = 0.0;
    MPI_Barrier(MPI_COMM_WORLD);
    double start = MPI_Wtime();
    for (int it = 0; it < iters; ++it)
    {
        double c = MPI_Wtime();
        // Обмін гало: перший вузол – лівому сусідові, від правого –
        // його перший вузол у u[local + 1]; потім у зворотний бік.
        MPI_Sendrecv(&u[1], 1, MPI_DOUBLE, left, 0,
                     &u[local + 1], 1, MPI_DOUBLE, right, 0,
                     MPI_COMM_WORLD, MPI_STATUS_IGNORE);
        MPI_Sendrecv(&u[local], 1, MPI_DOUBLE, right, 1,
                     &u[0], 1, MPI_DOUBLE, left, 1,
                     MPI_COMM_WORLD, MPI_STATUS_IGNORE);
        commTime += MPI_Wtime() - c;

        // Обчислення – потоками OpenMP усередині рангу.
        #pragma omp parallel for schedule(static)
        for (long i = 1; i <= local; ++i)
            v[i] = (u[i - 1] + u[i + 1] + b[i]) / (2 + Sigma);
        u.swap(v);
    }
    double time = MPI_Wtime() - start;

    // Найбільша похибка відносно точного розв'язку.
    double error = 0.0;
    #pragma omp parallel for reduction(max:error)
    for (long i = 1; i <= local; ++i)
        error = std::max(error,
                         std::abs(u[i] - Exact(first + i - 1, n)));
    MPI_Allreduce(MPI_IN_PLACE, &error, 1, MPI_DOUBLE, MPI_MAX,
                  MPI_COMM_WORLD);
    double times[2] = {time, commTime}, slowest[2];
    MPI_Reduce(times, slowest, 2, MPI_DOUBLE, MPI_MAX, 0,
               MPI_COMM_WORLD);
    if (rank == 0)
        std::println("{:>2} x {:<2} {:>8.3f} {:>8.3f} {:>10.2e}",
                     size, omp_get_max_threads(), slowest[0],
                     slowest[1], error);
    MPI_Finalize();
}
