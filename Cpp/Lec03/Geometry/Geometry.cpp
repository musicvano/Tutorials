#include <print>
#include <numbers>

[[nodiscard]] double area(double radius)
{
    return std::numbers::pi * radius * radius;
}

[[nodiscard]] double area(double length, double width)
{
    return length * width;
}

double cost(double square, double rate = 250.0)
{
    return square * rate;
}

int main()
{
    std::println("Circle: {:.2f}", area(2.0));
    std::println("Room: {:.2f}", area(5.0, 4.0));
    std::println("Cost: {:.2f}", cost(area(5.0, 4.0)));
    std::println("Other rate: {:.2f}", cost(20.0, 300.0));
}
