#include <cstdio>
int main()
{
    int* values = new int[4]{10, 20, 30, 40};

    int value = values[1];
    delete[] values;
    std::printf("%d\n", value);
}
