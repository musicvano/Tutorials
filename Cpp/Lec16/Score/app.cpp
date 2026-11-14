#include <print>
import score;
int main()
{
    for (int value : {-5, 70, 120}) {
        std::println("{} -> {}", value, limit_score(value));
    }
}
