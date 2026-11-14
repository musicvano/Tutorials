#include "fees.h"
#include <stdexcept>
int fees::delivery(int grams)
{
    if (grams <= 0 || grams > 5000) {
        throw std::invalid_argument("mass outside 1..5000");
    }
    return 40 + ((grams - 1) / 1000) * 10;
}
