// Та сама програма через бібліотечний модуль std (C++23).
// Потребує C/C++ -> Language -> Build ISO C++23 Standard Library Modules
// (<BuildStlModules> у проєкті). Основні приклади теми використовують
// #include <print>: шлях збирання однаковий в IDE та в команді cl.
import std;

int main()
{
    std::println("Привіт, C++26!");
    return 0;
}
