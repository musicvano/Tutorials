#include <vector>
#include <print>

int main()
{
    const std::vector<int> grades{2, 4, 5};
    int total{};
    for (int grade : grades) total += grade;
    const int count = static_cast<int>(grades.size());
    const double wrong = total / count;
    const double correct = static_cast<double>(total) / count;
    std::println("Wrong: {:.2f}", wrong);
    std::println("Correct: {:.2f}", correct);
}
