#include <cassert>
#include <cmath>
#include <memory>
#include <numbers>
#include <print>
#include <stdexcept>
#include <vector>
class Shape {
public:
    virtual ~Shape() = default;
    virtual double area() const { return 0; }
};
class Circle final : public Shape {
    double radius_;
public:
    explicit Circle(double r) : radius_(r) {
        if (!std::isfinite(r) || r <= 0 || r > 1000)
            throw std::invalid_argument("radius");
    }
    double area() const override {
        return std::numbers::pi * radius_ * radius_;
    }
};
class Rectangle final : public Shape {
    double width_, height_;
public:
    Rectangle(double w, double h) : width_(w), height_(h) {
        if (!std::isfinite(w) || !std::isfinite(h) ||
            w <= 0 || h <= 0 || w > 1000 || h > 1000)
            throw std::invalid_argument("sides");
    }
    double area() const override { return width_ * height_; }
};
int main() {
    std::vector<std::unique_ptr<Shape>> figures;
    figures.push_back(std::make_unique<Circle>(1));
    figures.push_back(std::make_unique<Rectangle>(2, 3));
    double sum = 0;
    for (const auto& figure : figures) sum += figure->area();
    assert(std::abs(sum - (6 + std::numbers::pi)) < 1e-12);
    try { Circle bad{0}; assert(false); }
    catch (const std::invalid_argument&) {}
    std::println("Сума площ: {:.3f}", sum);
}
