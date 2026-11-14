#include <mpi.h>
#include <cstdlib>
#include <print>
#include <string_view>
#include <vector>

// Кільцевий обмін: на кожному кроці ранг передає блок правому
// сусідові й отримує блок від лівого. Після size - 1 кроків кожен
// ранг має суму блоків усіх рангів.
// Аргументи: send | ssend | sendrecv | isend, кількість чисел.
int main(int argc, char* argv[])
{
    MPI_Init(&argc, &argv);
    int rank = 0, size = 0;
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);

    std::string_view mode = argc > 1 ? argv[1] : "sendrecv";
    int count = argc > 2 ? std::atoi(argv[2]) : 1000;
    int right = (rank + 1) % size;
    int left = (rank - 1 + size) % size;

    std::vector<double> out(count, rank + 1.0), in(count), sum = out;
    double start = MPI_Wtime();
    for (int step = 1; step < size; ++step)
    {
        if (mode == "send" || mode == "ssend")
        {   // Небезпечно: усі ранги спочатку відправляють.
            if (mode == "send")
                MPI_Send(out.data(), count, MPI_DOUBLE, right, 0,
                         MPI_COMM_WORLD);
            else
                MPI_Ssend(out.data(), count, MPI_DOUBLE, right, 0,
                          MPI_COMM_WORLD);
            MPI_Recv(in.data(), count, MPI_DOUBLE, left, 0,
                     MPI_COMM_WORLD, MPI_STATUS_IGNORE);
        }
        else if (mode == "sendrecv")
        {   // Відправлення й отримання однією операцією.
            MPI_Sendrecv(out.data(), count, MPI_DOUBLE, right, 0,
                         in.data(), count, MPI_DOUBLE, left, 0,
                         MPI_COMM_WORLD, MPI_STATUS_IGNORE);
        }
        else
        {   // Неблокуючі: спочатку обидва запити, потім Waitall.
            MPI_Request requests[2];
            MPI_Irecv(in.data(), count, MPI_DOUBLE, left, 0,
                      MPI_COMM_WORLD, &requests[0]);
            MPI_Isend(out.data(), count, MPI_DOUBLE, right, 0,
                      MPI_COMM_WORLD, &requests[1]);
            MPI_Waitall(2, requests, MPI_STATUSES_IGNORE);
        }
        for (int i = 0; i < count; ++i) sum[i] += in[i];
        out.swap(in);                       // далі передаємо отримане
    }
    double time = MPI_Wtime() - start;

    if (rank == 0)
        std::println("{:<8} {:>8} чисел: сума = {} (очікується {}), "
                     "{:.3f} мс", mode, count, sum[0],
                     size * (size + 1) / 2, time * 1e3);
    MPI_Finalize();
}
