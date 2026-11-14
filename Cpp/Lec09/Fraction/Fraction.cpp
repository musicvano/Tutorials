#include <cassert>
#include <compare>
#include <iostream>
#include <numeric>
#include <stdexcept>
class Fraction {
    long long n_, d_;
public:
    Fraction(long long n, long long d) : n_(n), d_(d) {
        if (n < -1'000'000 || n > 1'000'000 ||
            d < -1'000'000 || d > 1'000'000 || d == 0)
            throw std::invalid_argument("fraction range");
        if (d_ < 0) { n_ = -n_; d_ = -d_; }
        const auto g = std::gcd(n_, d_);
        n_ /= g; d_ /= g;
    }
    friend Fraction operator+(const Fraction& a,
                              const Fraction& b) {
        return {a.n_ * b.d_ + b.n_ * a.d_, a.d_ * b.d_};
    }
    bool operator==(const Fraction&) const = default;
    std::strong_ordering operator<=>(const Fraction& x) const {
        return n_ * x.d_ <=> x.n_ * d_;
    }
    friend std::ostream& operator<<(std::ostream& out,
                                    const Fraction& x) {
        return out << x.n_ << '/' << x.d_;
    }
};
int main() {
    Fraction a{1, 2}, b{1, 3};
    assert((a + b == Fraction{5, 6}));
    assert((Fraction{-2, -4} == a));
    assert((Fraction{0, 3} == Fraction{0, 1}));
    assert(a > b);
    try { Fraction bad{1, 0}; assert(false); }
    catch (const std::invalid_argument&) {}
    std::cout << a << " + " << b << " = " << a + b << '\n';
}
