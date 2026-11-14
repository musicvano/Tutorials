#include <array>
#include <cstddef>
#include <print>
#include <stdexcept>

template<class T, std::size_t N>
class FixedStack {
    std::array<T, N> data{};
    std::size_t used = 0;
public:
    FixedStack() = default;
    explicit FixedStack(const std::array<T, N>& source)
        : data(source), used(N) {}
    void push(const T& value) {
        if (used == N) throw std::length_error("full");
        data[used] = value;
        ++used;
    }
    T pop() {
        if (used == 0) throw std::out_of_range("empty");
        T result = data[used - 1];
        --used;
        return result;
    }
};

int main()
{
    FixedStack<int, 2> small;
    small.push(3);
    small.push(8);
    std::println("{}", small.pop());
    FixedStack inferred(std::array{10, 20, 30});
    std::println("{}", inferred.pop());
    FixedStack<int, 0> zero;
    try { zero.push(1); }
    catch (const std::length_error& e) {
        std::println("error: {}", e.what());
    }
}
