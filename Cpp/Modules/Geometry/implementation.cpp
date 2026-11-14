module;
#include <stdexcept>
module geometry;
double geometry::area(Rectangle value)
{
    if (value.width < 0 || value.height < 0) {
        throw std::invalid_argument("negative side");
    }
    return value.width * value.height;
}
