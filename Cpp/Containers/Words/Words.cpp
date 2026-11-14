#include <algorithm>
#include <print>
#include <sstream>
#include <string>
#include <unordered_map>
#include <utility>
#include <vector>

int main()
{
    std::istringstream input("red blue red green blue red");
    std::unordered_map<std::string, int> wordCount;
    for (std::string word; input >> word;)
        ++wordCount[word];
    std::vector<std::pair<std::string, int>> report;
    for (const auto& item : wordCount)
        report.push_back(item);
    std::sort(report.begin(), report.end());
    for (const auto& [word, count] : report)
        std::println("{}: {}", word, count);
}
