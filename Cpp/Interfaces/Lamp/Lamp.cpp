#include <cassert>
#include <print>
#include <stdexcept>
struct Switchable {
    virtual ~Switchable() = default;
    virtual void setOn(bool value) = 0;
};
struct Dimmable {
    virtual ~Dimmable() = default;
    virtual void brightness(int value) = 0;
};
struct Schedulable {
    virtual ~Schedulable() = default;
    virtual void schedule(int hour) = 0;
};
class SmartLamp final : public Switchable, public Dimmable,
                        public Schedulable {
    bool on_ = false;
    int brightness_ = 50, hour_ = 0;
public:
    void setOn(bool value) override { on_ = value; }
    void brightness(int n) override {
        if (n < 0 || n > 100) throw std::invalid_argument("light");
        brightness_ = n;
    }
    void schedule(int h) override {
        if (h < 0 || h > 23) throw std::invalid_argument("hour");
        hour_ = h;
    }
    int effective() const { return on_ ? brightness_ : 0; }
    int hour() const { return hour_; }
};
int main() {
    SmartLamp lamp;
    Switchable& power = lamp; power.setOn(true);
    Dimmable& light = lamp; light.brightness(80);
    Schedulable& plan = lamp; plan.schedule(18);
    try { light.brightness(101); assert(false); }
    catch (const std::invalid_argument&) {}
    assert(lamp.effective() == 80 && lamp.hour() == 18);
    power.setOn(false); assert(lamp.effective() == 0);
    std::println("Розклад: {}; світло: {}",
        lamp.hour(), lamp.effective());
}
