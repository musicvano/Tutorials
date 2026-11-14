#include <memory>
#include <vector>
#include <string>
#include <print>

struct Song { std::string title; };

int main()
{
    auto song = std::make_shared<Song>(Song{"Morning"});
    std::weak_ptr<Song> recent = song;
    std::vector<std::shared_ptr<Song>> first{song};
    std::vector<std::shared_ptr<Song>> second{song};

    std::println("Owners: {}", song.use_count());
    song.reset();
    first.clear();
    if (auto active = recent.lock())
        std::println("Available: {}", active->title);
    second.clear();
    std::println("Expired: {}", recent.expired());
}
