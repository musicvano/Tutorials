#include <mpi.h>
#include <print>
#include <string_view>

int main(int argc, char* argv[])
{
    MPI_Init(&argc, &argv);

    int size = 0, rank = 0;
    MPI_Comm_size(MPI_COMM_WORLD, &size);   // кількість процесів
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);   // номер цього процесу

    char host[MPI_MAX_PROCESSOR_NAME];
    int length = 0;
    MPI_Get_processor_name(host, &length);

    if (rank == 0)
    {
        char version[MPI_MAX_LIBRARY_VERSION_STRING];
        MPI_Get_library_version(version, &length);
        std::string_view text(version);       // до першої коми
        std::println("MPI {}.{}, {}", MPI_VERSION, MPI_SUBVERSION,
                     text.substr(0, text.find(',')));
    }
    std::println("Rank {} of {} on {}", rank, size, host);

    MPI_Finalize();
}
