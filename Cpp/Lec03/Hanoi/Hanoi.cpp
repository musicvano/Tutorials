#include <print>
#include <iostream>

void hanoi(int n, char from, char to, char spare, int& steps)
{
    if (n == 0) return;
    hanoi(n - 1, from, spare, to, steps);
    std::println("{}: {} -> {}", ++steps, from, to);
    hanoi(n - 1, spare, to, from, steps);
}

int main()
{
    int n{};
    if (!(std::cin >> n) || n < 0 || n > 10)
    {
        std::cerr << "Expected disk count 0..10\n";
        return 1;
    }
    int steps{};
    hanoi(n, 'A', 'C', 'B', steps);
    std::println("Total: {}", steps);
}
