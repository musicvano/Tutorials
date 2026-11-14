#include <omp.h>
#include <print>

int main()
{
    std::println("OpenMP {}, процесорів: {}, потоків: {}",
                 _OPENMP, omp_get_num_procs(),
                 omp_get_max_threads());

    // Послідовна частина: працює лише головний потік.
    int answer = 42;

    #pragma omp parallel num_threads(8)
    {
        // Змінні, оголошені всередині області, – приватні.
        int id = omp_get_thread_num();
        int count = omp_get_num_threads();
        #pragma omp critical
        std::println("Hello from thread {} of {} (answer = {})",
                     id, count, answer);
    }

    std::println("Знову один потік: {} з {}",
                 omp_get_thread_num(), omp_get_num_threads());
}
