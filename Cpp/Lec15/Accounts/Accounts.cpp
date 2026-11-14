#include <cstdint>
#include <fstream>
#include <print>
#include <type_traits>

struct Record { std::uint32_t id; std::int64_t cents; };
static_assert(std::is_trivially_copyable_v<Record>);
int main()
{
    Record rows[]{{1, 1000}, {2, 2500}};
    {
        std::ofstream out("accounts-demo.dat",
            std::ios::binary | std::ios::trunc);
        out.write(reinterpret_cast<const char*>(rows),
            sizeof rows);
        out.close();
        if (!out) return 1;
    }
    std::fstream file("accounts-demo.dat",
        std::ios::in | std::ios::out | std::ios::binary);
    if (!file) return 1;
    auto offset = static_cast<std::streamoff>(sizeof(Record));
    file.seekg(offset);
    Record item{};
    if (!file.read(reinterpret_cast<char*>(&item),
        sizeof item)) return 1;
    item.cents += 500;
    file.seekp(offset);
    file.write(reinterpret_cast<const char*>(&item),
        sizeof item);
    file.flush();
    if (!file) return 1;
    file.seekg(offset);
    if (!file.read(reinterpret_cast<char*>(&item),
        sizeof item)) return 1;
    std::println("id: {}, cents: {}", item.id, item.cents);
    file.close();
    return file ? 0 : 1;
}
