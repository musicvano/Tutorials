#include <algorithm>
#include <chrono>
#include <cmath>
#include <cstddef>
#include <numeric>
#include <print>
#include <random>
#include <string>
#include <thread>
#include <vector>

using Clock = std::chrono::steady_clock;

// Сума квадратних коренів елементів з індексами [from, to).
double sum_range(const std::vector<double>& v,
                 std::size_t from, std::size_t to) {
    double sum = 0.0;
    for (std::size_t i = from; i < to; ++i) sum += std::sqrt(v[i]);
    return sum;
}

double parallel_sum(const std::vector<double>& v, unsigned p) {
    std::vector<double> parts(p);          // результат кожного потоку
    {
        std::vector<std::jthread> threads;
        std::size_t chunk = v.size() / p;
        for (unsigned k = 0; k < p; ++k) {
            std::size_t from = k * chunk;
            std::size_t to = k + 1 == p ? v.size() : from + chunk;
            threads.emplace_back([&v, &parts, k, from, to] {
                parts[k] = sum_range(v, from, to);
            });
        }
    }   // тут деструктори jthread чекають завершення потоків
    return std::accumulate(parts.begin(), parts.end(), 0.0);
}

// Медіана часу (мс) п’яти запусків; result – сума останнього.
double median_ms(const std::vector<double>& v, unsigned p,
                 double& result) {
    std::vector<double> times;
    for (int run = 0; run < 5; ++run) {
        auto start = Clock::now();
        result = parallel_sum(v, p);
        std::chrono::duration<double, std::milli> ms =
            Clock::now() - start;
        times.push_back(ms.count());
    }
    std::ranges::sort(times);
    return times[2];
}

int main(int argc, char* argv[]) {
    std::size_t n = argc > 1 ? std::stoull(argv[1]) : 100'000'000;
    unsigned cores = std::thread::hardware_concurrency();

    std::vector<double> v(n);
    std::mt19937_64 gen(42);                     // фіксоване зерно
    std::uniform_real_distribution<double> dist(0.0, 100.0);
    std::ranges::generate(v, [&] { return dist(gen); });

    std::println("n = {}, логічних процесорів: {}", n, cores);
    double expected = sum_range(v, 0, n);        // також прогрівання
    double t1 = 0.0;
    std::println("{:>3} {:>9} {:>6} {:>6} {:>10}",
                 "p", "T, мс", "S", "E", "похибка");
    for (unsigned p = 1; p <= cores; p *= 2) {
        double sum = 0.0;
        double tp = median_ms(v, p, sum);
        if (p == 1) t1 = tp;
        double s = t1 / tp;
        std::println("{:>3} {:>9.1f} {:>6.2f} {:>5.0f}% {:>10.1e}",
                     p, tp, s, 100 * s / p,
                     std::abs(sum - expected) / expected);
    }
}
