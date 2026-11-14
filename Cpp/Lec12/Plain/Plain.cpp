template<class T>
concept Addable = requires(T a, T b) { a + b; };

template<class T> requires Addable<T>
T add(T a, T b) { return a + b; }

struct Record { friend Record operator+(Record, Record) { return {}; } };
int main()
{
    auto result = add(Record{}, Record{});
    (void)result;
}
