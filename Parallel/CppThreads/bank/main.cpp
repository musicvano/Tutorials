#include <atomic>
#include <chrono>
#include <mutex>
#include <print>
#include <thread>
#include <vector>

const int Threads = 8;
const int Deposits = 1'000'000;          // поповнень у потоці

// Суми поповнень (по 1 грн): запис у пам’ять, а не лише в регістр.
std::vector<long long> amounts(Deposits, 1);

// Запускає Threads потоків, кожен викликає deposit Deposits разів.
template <typename F>
double run(F deposit) {
    auto start = std::chrono::steady_clock::now();
    {
        std::vector<std::jthread> threads;
        for (int t = 0; t < Threads; ++t)
            threads.emplace_back([&] {
                for (int i = 0; i < Deposits; ++i)
                    deposit(amounts[i]);
            });
    }
    std::chrono::duration<double, std::milli> ms =
        std::chrono::steady_clock::now() - start;
    return ms.count();
}

struct Account {
    int id;
    long long balance;
    std::mutex m;
};

// Переказ: scoped_lock блокує обидва м’ютекси без взаємоблокування.
void transfer(Account& from, Account& to, long long amount) {
    std::scoped_lock lock(from.m, to.m);
    if (from.balance >= amount) {
        from.balance -= amount;
        to.balance += amount;
    }
}

int main() {
    long long expected = 1LL * Threads * Deposits;
    std::println("Очікуваний баланс: {}", expected);

    long long unsafe = 0;                        // гонитва!
    double t = run([&](long long x) { unsafe += x; });
    std::println("Без синхронізації: {:>9} ({:.0f} мс)", unsafe, t);

    long long guarded = 0;
    std::mutex m;
    t = run([&](long long x) {
        std::lock_guard lock(m);
        guarded += x;
    });
    std::println("std::mutex:        {:>9} ({:.0f} мс)", guarded, t);

    std::atomic<long long> atomic = 0;
    t = run([&](long long x) { atomic += x; });
    std::println("std::atomic:       {:>9} ({:.0f} мс)",
                 atomic.load(), t);

    // Зустрічні перекази між двома рахунками з двох потоків.
    Account a{1, 1'000'000, {}}, b{2, 1'000'000, {}};
    {
        std::jthread ab([&] {
            for (int i = 0; i < Deposits; ++i) transfer(a, b, 30);
        });
        std::jthread ba([&] {
            for (int i = 0; i < Deposits; ++i) transfer(b, a, 20);
        });
    }
    std::println("Рахунки: {} + {} = {}", a.balance, b.balance,
                 a.balance + b.balance);
}
