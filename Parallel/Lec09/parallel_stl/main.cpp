#include <algorithm>
#include <chrono>
#include <cmath>
#include <execution>
#include <functional>
#include <print>
#include <random>
#include <string_view>
#include <vector>

namespace ex = std::execution;
using Data = std::vector<double>;

// Медіана часу (мс) п’яти запусків action(копія data).
double median_ms(const Data& data, auto action) {
    std::vector<double> times;
    for (int run = 0; run < 5; ++run) {
        Data copy = data;
        auto start = std::chrono::steady_clock::now();
        action(copy);
        std::chrono::duration<double, std::milli> ms =
            std::chrono::steady_clock::now() - start;
        times.push_back(ms.count());
    }
    std::ranges::sort(times);
    return times[2];
}

void report(std::string_view name, double t, double t_seq) {
    std::println("  {:<10} {:>8.1f} мс  S = {:.2f}", name, t,
                 t_seq / t);
}

// Сума exp(-x²) за політикою policy.
double gauss_sum(const auto& policy, const Data& v) {
    return std::transform_reduce(policy, v.begin(), v.end(), 0.0,
        std::plus<>{}, [](double x) { return std::exp(-x * x); });
}

int main() {
    const std::size_t N = 20'000'000;
    Data data(N);
    std::mt19937_64 gen(2026);
    std::uniform_real_distribution<double> dist(-3.0, 3.0);
    std::ranges::generate(data, [&] { return dist(gen); });

    std::println("sort, {} елементів:", N);
    double seq = median_ms(data, [](Data& v) {
        std::sort(ex::seq, v.begin(), v.end());
    });
    report("seq", seq, seq);
    double par = median_ms(data, [](Data& v) {
        std::sort(ex::par, v.begin(), v.end());
    });
    report("par", par, seq);

    std::println("transform_reduce, сума exp(-x²):");
    double sums[4];
    seq = median_ms(data, [&](Data& v) {
        sums[0] = gauss_sum(ex::seq, v); });
    report("seq", seq, seq);
    report("unseq", median_ms(data, [&](Data& v) {
        sums[1] = gauss_sum(ex::unseq, v); }), seq);
    report("par", median_ms(data, [&](Data& v) {
        sums[2] = gauss_sum(ex::par, v); }), seq);
    report("par_unseq", median_ms(data, [&](Data& v) {
        sums[3] = gauss_sum(ex::par_unseq, v); }), seq);
    double deviation = 0;
    for (double s : sums)
        deviation =
            std::max(deviation, std::abs(s - sums[0]) / sums[0]);
    std::println("Сума: {:.4f}, найбільше відхилення: {:.1e}",
                 sums[0], deviation);
}
