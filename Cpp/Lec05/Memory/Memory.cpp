#include <print>
int main()
{
    int values[]{10, 20, 30, 40};
    int* pointer = values;

    std::println("{}", *pointer);
}
