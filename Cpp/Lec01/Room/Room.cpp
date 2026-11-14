#include <print>

int main()
{
    const double length = 5.0;
    const double width = 4.0;
    const double price = 250.0;
    const double area = length * width;
    const double perimeter = 2.0 * (length + width);
    const double total = area * price;

    std::println("Площа: {:.2f} м²", area);
    std::println("Периметр: {:.2f} м", perimeter);
    std::println("Вартість: {:.2f} грн", total);
    return 0;
}
