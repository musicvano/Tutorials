#include <print>
#include <iostream>
#include <limits>
#include <cmath>

int main()
{
    double mass{}, height{};
    while (true)
    {
        std::println("Mass kg (0 < value <= 300):");
        if (std::cin >> mass)
        {
            std::cin.ignore(
                std::numeric_limits<std::streamsize>::max(), '\n');
            if (std::isfinite(mass) && mass > 0 && mass <= 300)
                break;
        }
        else if (std::cin.eof()) return 1;
        else
        {
            std::cin.clear();
            std::cin.ignore(
                std::numeric_limits<std::streamsize>::max(), '\n');
        }
        std::println("Invalid mass");
    }
    std::println("Height m (0 < value <= 3):");
    if (!(std::cin >> height) || !std::isfinite(height)
        || height <= 0 || height > 3)
    {
        std::cerr << "Invalid height\n";
        return 1;
    }
    const double bmi = mass / (height * height);
    std::println("BMI = {:.2f}", bmi);
}
