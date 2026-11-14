#include "geometry.h"
#include <stdexcept>
double geometry::rectangle(double width, double height)
{
    if (width < 0 || height < 0) {
        throw std::invalid_argument("negative side");
    }
    return width * height;
}
