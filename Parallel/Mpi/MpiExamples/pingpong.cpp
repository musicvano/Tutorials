#include <mpi.h>
#include <print>
#include <vector>

// Пінг-понг між рангами 0 і 1: час передавання повідомлення в один
// бік для розмірів від 1 байта до 16 МБ (крок ×16) і пропускна
// здатність. Модель α–β: T(m) = α + β·m.
// (Відтворено за описом лекції: лекція друкує лише цикл обміну.)
int main(int argc, char* argv[])
{
    MPI_Init(&argc, &argv);
    int rank = 0, size = 0;
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);
    if (size < 2)
    {
        if (rank == 0) std::println("Потрібно щонайменше 2 ранги");
        MPI_Finalize();
        return 1;
    }

    if (rank == 0)
        std::println("{:>10} {:>12} {:>10}", "Байтів", "мкс", "МБ/с");
    for (int bytes = 1; bytes <= 16 * 1024 * 1024; bytes *= 16)
    {
        std::vector<char> buffer(bytes, 'x');
        int reps = bytes <= 65536 ? 1000 : 50;
        MPI_Barrier(MPI_COMM_WORLD);
        double start = MPI_Wtime();
        for (int r = 0; r < reps; ++r)
        {
            if (rank == 0)
            {
                MPI_Send(buffer.data(), bytes, MPI_CHAR, 1, 0,
                         MPI_COMM_WORLD);
                MPI_Recv(buffer.data(), bytes, MPI_CHAR, 1, 0, MPI_COMM_WORLD,
                         MPI_STATUS_IGNORE);
            }
            else if (rank == 1)
            {
                MPI_Recv(buffer.data(), bytes, MPI_CHAR, 0, 0, MPI_COMM_WORLD,
                         MPI_STATUS_IGNORE);
                MPI_Send(buffer.data(), bytes, MPI_CHAR, 0, 0,
                         MPI_COMM_WORLD);
            }
        }
        double t = (MPI_Wtime() - start) / reps / 2;   // в один бік
        if (rank == 0)
            std::println("{:>10} {:>12.2f} {:>10.0f}", bytes, t * 1e6,
                         bytes / t / 1e6);
    }
    MPI_Finalize();
}
