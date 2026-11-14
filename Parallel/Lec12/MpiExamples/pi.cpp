#include <mpi.h>
#include <algorithm>
#include <cmath>
#include <cstdlib>
#include <numbers>
#include <print>

// Частина суми методу середніх прямокутників для цього рангу:
// ітерації rank, rank + size, rank + 2·size, … (циклічний розподіл).
double PartialSum(long long steps, int rank, int size)
{
    const double h = 1.0 / steps;
    double sum = 0.0;
    for (long long i = rank; i < steps; i += size)
    {
        double x = (i + 0.5) * h;
        sum += 4.0 / (1.0 + x * x);
    }
    return sum * h;
}

int main(int argc, char* argv[])
{
    MPI_Init(&argc, &argv);
    int rank = 0, size = 0;
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);

    // Кількість кроків знає лише ранг 0 і розсилає її всім.
    long long steps = 0;
    if (rank == 0)
        steps = argc > 1 ? std::atoll(argv[1]) : 2'000'000'000LL;
    MPI_Bcast(&steps, 1, MPI_LONG_LONG, 0, MPI_COMM_WORLD);

    double pi = 0.0, times[5];
    for (double& t : times)                   // медіана 5 запусків
    {
        MPI_Barrier(MPI_COMM_WORLD);          // спільний старт
        double start = MPI_Wtime();
        double local = PartialSum(steps, rank, size);
        MPI_Reduce(&local, &pi, 1, MPI_DOUBLE, MPI_SUM, 0,
                   MPI_COMM_WORLD);
        t = MPI_Wtime() - start;
    }
    // Час запуску – час найповільнішого рангу.
    double slowest[5];
    MPI_Reduce(times, slowest, 5, MPI_DOUBLE, MPI_MAX, 0,
               MPI_COMM_WORLD);
    if (rank == 0)
    {
        std::sort(slowest, slowest + 5);
        std::println("{} {:.4f} {:.12f} {:.1e}", size, slowest[2], pi,
                     std::abs(pi - std::numbers::pi));
    }
    MPI_Finalize();
}
