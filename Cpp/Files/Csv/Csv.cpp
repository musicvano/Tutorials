#include <fstream>
#include <print>
#include <sstream>
#include <string>

int main()
{
    {
        std::ofstream seed("grades-demo.csv");
        seed << "Anna,90\nOleh,75\nBad,abc\nIra,101\n";
        seed.close();
        if (!seed) return 1;
    }
    std::ifstream input("grades-demo.csv");
    std::ofstream report("grades-report.txt");
    if (!input || !report) return 1;
    int accepted = 0, rejected = 0, sum = 0;
    for (std::string line; std::getline(input, line);) {
        std::istringstream row(line);
        std::string name, tail;
        int grade = 0;
        if (!std::getline(row, name, ',') || name.empty()
            || !(row >> grade) || (row >> tail)
            || grade < 0 || grade > 100) {
            ++rejected;
            continue;
        }
        ++accepted; sum += grade;
        report << name << ": " << grade << '\n';
    }
    if (input.bad()) return 1;
    report.close();
    if (!report) return 1;
    std::println("accepted: {}, rejected: {}",
        accepted, rejected);
    if (accepted != 0)
        std::println("mean: {:.2f}",
            static_cast<double>(sum) / accepted);
}
