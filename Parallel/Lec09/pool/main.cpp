#include <condition_variable>
#include <functional>
#include <future>
#include <mutex>
#include <print>
#include <queue>
#include <stdexcept>
#include <thread>
#include <vector>

class ThreadPool {
public:
    explicit ThreadPool(unsigned count) {
        for (unsigned i = 0; i < count; ++i)
            workers_.emplace_back([this] { work(); });
    }

    ~ThreadPool() {
        {
            std::lock_guard lock(mutex_);
            stopping_ = true;
        }
        ready_.notify_all();      // розбудити всіх для завершення
    }                             // далі деструктори jthread: join()

    // Ставить функцію в чергу й повертає future її результату.
    template <typename F>
    auto submit(F f) -> std::future<decltype(f())> {
        std::packaged_task<decltype(f())()> task(std::move(f));
        auto result = task.get_future();
        {
            std::lock_guard lock(mutex_);
            tasks_.emplace(std::move(task));
        }
        ready_.notify_one();      // розбудити один робочий потік
        return result;
    }

private:
    void work() {
        while (true) {
            std::move_only_function<void()> task;
            {
                std::unique_lock lock(mutex_);
                ready_.wait(lock, [this] {
                    return stopping_ || !tasks_.empty();
                });
                if (tasks_.empty()) return;   // черга порожня
                task = std::move(tasks_.front());
                tasks_.pop();
            }
            task();               // виконується без блокування
        }
    }

    std::mutex mutex_;
    std::condition_variable ready_;
    std::queue<std::move_only_function<void()>> tasks_;
    bool stopping_ = false;
    std::vector<std::jthread> workers_;  // останнє: руйнується першим
};

// Кількість простих чисел у діапазоні [from, to).
int count_primes(int from, int to) {
    if (from > to) throw std::invalid_argument("from > to");
    int count = 0;
    for (int n = std::max(from, 2); n < to; ++n) {
        bool prime = true;
        for (int d = 2; d * d <= n && prime; ++d) prime = n % d != 0;
        count += prime;
    }
    return count;
}

int main() {
    ThreadPool pool(4);
    const int Step = 2'500'000;
    std::vector<std::future<int>> results;
    for (int k = 0; k < 4; ++k)
        results.push_back(pool.submit([k] {
            return count_primes(k * Step, (k + 1) * Step);
        }));
    auto wrong = pool.submit([] { return count_primes(10, 1); });

    int total = 0;
    for (int k = 0; k < 4; ++k) {
        int count = results[k].get();       // чекає на результат
        std::println("[{:>8}; {:>8}): {:>6}", k * Step,
                     (k + 1) * Step, count);
        total += count;
    }
    std::println("Разом простих до {}: {}", 4 * Step, total);
    try {
        wrong.get();                        // виняток із потоку пулу
    } catch (const std::invalid_argument& e) {
        std::println("Помилка задачі: {}", e.what());
    }
}
