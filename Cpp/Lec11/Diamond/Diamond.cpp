#include <cassert>
#include <print>
#include <string>
#include <utility>
struct Person {
    std::string name;
    explicit Person(std::string value) : name(std::move(value)) {}
};
struct Student : virtual Person {
    Student() : Person("student") {}
};
struct Employee : virtual Person {
    Employee() : Person("employee") {}
};
struct TeachingAssistant final : Student, Employee {
    explicit TeachingAssistant(std::string name)
        : Person(std::move(name)), Student(), Employee() {}
};
int main() {
    TeachingAssistant assistant{"Олена"};
    Person* viaStudent = static_cast<Student*>(&assistant);
    Person* viaEmployee = static_cast<Employee*>(&assistant);
    assert(viaStudent == viaEmployee);
    assert(assistant.name == "Олена");
    viaStudent->name = "Марія";
    assert(viaEmployee->name == "Марія");
    std::println("Спільна людина: {}", assistant.name);
}
