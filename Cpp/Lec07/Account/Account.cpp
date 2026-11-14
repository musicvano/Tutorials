#include <cassert>
#include <print>
#include <stdexcept>
class BankAccount {
    int balance_;
public:
    static constexpr int limit = 1'000'000;
    explicit BankAccount(int value) : balance_(value) {
        if (value < 0 || value > limit)
            throw std::invalid_argument("balance");
    }
    int balance() const { return balance_; }
    void deposit(int value) {
        if (value <= 0 || value > limit - balance_)
            throw std::invalid_argument("deposit");
        balance_ += value;
    }
    void withdraw(int value) {
        if (value <= 0 || value > balance_)
            throw std::invalid_argument("withdraw");
        balance_ -= value;
    }
};
int main() {
    BankAccount account{100};
    account.deposit(50);
    account.withdraw(20);
    try { account.withdraw(131); }
    catch (const std::invalid_argument&) {
        std::println("Відмовлено");
    }
    assert(account.balance() == 130);
    account.withdraw(130);
    assert(account.balance() == 0);
    try { BankAccount bad{-1}; assert(false); }
    catch (const std::invalid_argument&) {}
    try { account.deposit(0); assert(false); }
    catch (const std::invalid_argument&) {}
    std::println("Залишок: {}", account.balance());
}
