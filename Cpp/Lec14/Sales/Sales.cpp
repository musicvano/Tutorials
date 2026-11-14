#include <algorithm>
#include <numeric>
#include <print>
#include <vector>

int main()
{
    std::vector<int> sales{30, 10, 20, 10};
    auto sum = std::accumulate(sales.begin(), sales.end(), 0);
    auto count = std::count_if(sales.begin(), sales.end(),
        [](int value) { return value >= 20; });
    std::sort(sales.begin(), sales.end(),
        [](int x, int y) {
            return x > y;
        });
    std::println("sum: {}, large: {}", sum, count);
    for (int value : sales) std::print("{} ", value);
    std::println();
}
