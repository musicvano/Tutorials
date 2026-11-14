#include <cassert>
#include <print>
#include <vector>
struct Safe {
    inline static int copies = 0, moves = 0;
    Safe() = default;
    Safe(const Safe&) { ++copies; }
    Safe(Safe&&) noexcept { ++moves; }
};
struct Risky {
    inline static int copies = 0, moves = 0;
    Risky() = default;
    Risky(const Risky&) { ++copies; }
    Risky(Risky&&) noexcept(false) { ++moves; }
};
int main() {
    std::vector<Safe> a(1);
    a.reserve(a.capacity() + 1);
    std::vector<Risky> b(1);
    b.reserve(b.capacity() + 1);
    assert(Safe::moves == 1 && Safe::copies == 0);
    assert(Risky::copies == 1 && Risky::moves == 0);
    std::println("Safe: copy {}, move {}",
        Safe::copies, Safe::moves);
    std::println("Risky: copy {}, move {}",
        Risky::copies, Risky::moves);
}
