#include <omp.h>
#include <algorithm>
#include <cmath>
#include <numbers>
#include <print>

const long long Steps = 1'000'000'000;

// π як інтеграл 4/(1 + x²) на [0; 1] методом середніх прямокутників.
double ComputePi(int threads)
{
    const double h = 1.0 / Steps;
    double sum = 0.0;
    #pragma omp parallel for num_threads(threads) reduction(+:sum)
    for (long long i = 0; i < Steps; ++i)
    {
        double x = (i + 0.5) * h;
        sum += 4.0 / (1.0 + x * x);
    }
    return sum * h;
}

// Медіана п’яти вимірювань за годинником omp_get_wtime.
double MedianTime(int threads, double& pi)
{
    double t[5];
    for (double& ti : t)
    {
        double start = omp_get_wtime();
        pi = ComputePi(threads);
        ti = omp_get_wtime() - start;
    }
    std::sort(t, t + 5);
    return t[2];
}

int main()
{
    int maxThreads = omp_get_num_procs();
    double pi = 0.0;
    ComputePi(1);                               // прогрівання
    ComputePi(maxThreads);
    double t1 = MedianTime(1, pi);

    std::println("Кроків: {}, процесорів: {}", Steps, maxThreads);
    std::println("{:>3} {:>9} {:>6} {:>6} {:>14}",
                 "p", "Tp, с", "S", "E", "похибка");
    for (int p = 1; p <= maxThreads; p *= 2)
    {
        double tp = p == 1 ? t1 : MedianTime(p, pi);
        double s = t1 / tp;
        std::println("{:>3} {:>9.3f} {:>6.2f} {:>5.0f}% {:>14.2e}",
                     p, tp, s, 100 * s / p,
                     std::abs(pi - std::numbers::pi));
    }
}
