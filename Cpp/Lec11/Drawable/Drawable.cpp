#include <cassert>
#include <cmath>
#include <numbers>
#include <print>
#include <stdexcept>
#include <string>
struct Drawable {
    virtual ~Drawable() = default;
    virtual std::string draw() const = 0;
};
struct Resizable {
    virtual ~Resizable() = default;
    virtual void resize(double factor) = 0;
};
struct Shape : Drawable {
    virtual double area() const = 0;
};
class Circle final : public Shape, public Resizable {
    double radius_;
public:
    explicit Circle(double r) : radius_(r) {
        if (!std::isfinite(r) || r <= 0 || r > 1000)
            throw std::invalid_argument("radius");
    }
    void resize(double k) override {
        if (!std::isfinite(k) || k <= 0 ||
            k > 1000 / radius_ || radius_ * k == 0)
            throw std::invalid_argument("factor");
        radius_ *= k;
    }
    double area() const override {
        return std::numbers::pi * radius_ * radius_;
    }
    std::string draw() const override { return "Circle"; }
};
int main() {
    Circle c{1};
    Resizable& sizing = c; sizing.resize(2);
    const Drawable& drawing = c;
    assert(std::abs(c.area() - 4 * std::numbers::pi) < 1e-12);
    try { sizing.resize(0); assert(false); }
    catch (const std::invalid_argument&) {}
    std::println("{}: {:.3f}", drawing.draw(), c.area());
}
