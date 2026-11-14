#include <cassert>
#include <iostream>
#include <stdexcept>
class Money {
    long long cents_;
    static constexpr long long limit = 1'000'000'000;
public:
    explicit Money(long long n = 0) : cents_(n) {
        if (n < 0 || n > limit)
            throw std::invalid_argument("money");
    }
    Money& operator+=(Money b) {
        if (b.cents_ > limit - cents_)
            throw std::overflow_error("sum");
        cents_ += b.cents_; return *this;
    }
    friend Money operator+(Money a, Money b) { return a += b; }
    friend Money operator*(Money a, int n) {
        if (n < 0 || (n > 0 && a.cents_ > limit / n))
            throw std::invalid_argument("count");
        return Money{a.cents_ * n};
    }
    friend Money operator*(int n, Money a) { return a * n; }
    bool operator==(const Money&) const = default;
    friend std::ostream& operator<<(std::ostream& out, Money a) {
        return out << a.cents_ << " коп.";
    }
};
int main() {
    Money a{1250};
    assert(a * 3 == 3 * a);
    assert(a * 0 == Money{});
    a += Money{50};
    assert(a == Money{1300});
    try { auto bad = a * -1; (void)bad; assert(false); }
    catch (const std::invalid_argument&) {}
    std::cout << a * 3 << '\n';
}
