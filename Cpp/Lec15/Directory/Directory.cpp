#include <filesystem>
#include <fstream>
#include <map>
#include <print>
#include <string>
#include <system_error>

namespace fs = std::filesystem;
int main()
{
    const fs::path root = "directory-demo";
    fs::create_directories(root / "sub");
    for (const auto& [name, data] :
        std::map<std::string, std::string>{
        {"a.cpp", "abc"}, {"b.txt", "hello"},
        {"sub/c.cpp", "1234567"}}) {
        std::ofstream out(root / name, std::ios::binary);
        out << data;
        out.close();
        if (!out) return 1;
    }
    std::map<std::string, std::uintmax_t> totals;
    std::error_code ec;
    fs::recursive_directory_iterator it(root, ec), end;
    if (ec) { std::println("open: {}", ec.message()); return 1; }
    while (it != end) {
        bool regular = it->is_regular_file(ec);
        if (ec) { std::println("type: {}", ec.message()); return 1; }
        if (regular) {
            auto size = it->file_size(ec);
            if (ec) return 1;
            totals[it->path().extension().string()] += size;
        }
        it.increment(ec);
        if (ec) { std::println("next: {}", ec.message()); return 1; }
    }
    for (const auto& [ext, bytes] : totals)
        std::println("{}: {} bytes", ext, bytes);
}
