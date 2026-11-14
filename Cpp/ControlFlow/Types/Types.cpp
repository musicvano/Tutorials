#include <print>
#include <limits>

int main()
{
    constexpr auto limit = std::numeric_limits<int>::max();
    std::println("int: {} bytes, {} .. {}", sizeof(int),
        std::numeric_limits<int>::min(),
        limit);
    std::println("long: {} bytes", sizeof(long));
    std::println("long long: {} bytes", sizeof(long long));
    std::println("double: {} bytes, {} digits", sizeof(double),
        std::numeric_limits<double>::digits10);
}
