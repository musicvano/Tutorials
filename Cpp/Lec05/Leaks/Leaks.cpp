#define _CRTDBG_MAP_ALLOC
#include <cstdlib>
#include <crtdbg.h>
int main()
{
    int* values = new int[4]{10, 20, 30, 40};
    delete[] values;
    _CrtDumpMemoryLeaks();

}
