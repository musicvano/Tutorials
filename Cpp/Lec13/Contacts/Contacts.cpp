#include <map>
#include <print>
#include <string>

int main()
{
    std::map<std::string, std::string> phoneBook;
    phoneBook.try_emplace("Olena", "101");
    auto [it, added] = phoneBook.try_emplace("Olena", "999");
    std::println("added: {}, phone: {}", added, it->second);
    phoneBook.insert_or_assign("Taras", "202");
    if (auto found = phoneBook.find("Taras");
        found != phoneBook.end())
        std::println("found: {}", found->second);
    phoneBook.erase("Taras");
    for (const auto& [name, phone] : phoneBook)
        std::println("{}: {}", name, phone);
    std::println("missing: {}", phoneBook.contains("Ira"));
}
