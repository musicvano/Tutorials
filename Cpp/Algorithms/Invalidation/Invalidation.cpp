#include <vector>
int main()
{
    std::vector<int> values{1, 2, 3};
    auto it = values.begin();
    values.reserve(values.capacity() + 1);
    it = values.begin();
    return *it == 1 ? 0 : 1;
}
