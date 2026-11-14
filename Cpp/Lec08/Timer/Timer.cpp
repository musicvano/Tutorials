#include <cassert>
#include <chrono>
#include <print>
#include <stdexcept>
class ScopeTimer {
    using Clock = std::chrono::steady_clock;
    Clock::time_point start_ = Clock::now();
    double& result_;
public:
    explicit ScopeTimer(double& result) : result_(result) {}
    ScopeTimer(const ScopeTimer&) = delete;
    ScopeTimer& operator=(const ScopeTimer&) = delete;
    ~ScopeTimer() noexcept {
        result_ = std::chrono::duration<double>(
            Clock::now() - start_).count();
    }
};
int main() {
    double elapsed = -1;
    try {
        ScopeTimer timer{elapsed};
        throw std::runtime_error("test");
    } catch (const std::runtime_error&) {}
    assert(elapsed >= 0);
    std::println("Таймер завершено: {}", elapsed >= 0);
}
