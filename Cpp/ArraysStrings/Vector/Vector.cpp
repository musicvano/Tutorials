#include <vector>
#include <string>
#include <print>

int main()
{
    std::vector<int> values{10, 20, 30};
    values.reserve(8);
    std::string text = "First line\nSecond line\nThird line";

    std::println("{}", text);
}
