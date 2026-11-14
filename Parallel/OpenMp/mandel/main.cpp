#include <omp.h>
#include <algorithm>
#include <print>
#include <string>
#include <vector>

const int Width = 2400, Height = 1600, MaxIter = 2000;

// Кількість ітерацій для рядка y: чорні точки множини – найдовші.
long long Row(int y)
{
    long long total = 0;
    double ci = -1.0 + 2.0 * y / Height;
    for (int x = 0; x < Width; ++x)
    {
        double cr = -2.2 + 3.0 * x / Width;
        double zr = 0, zi = 0;
        int k = 0;
        while (k < MaxIter && zr * zr + zi * zi <= 4.0)
        {
            double t = zr * zr - zi * zi + cr;
            zi = 2 * zr * zi + ci;
            zr = t;
            ++k;
        }
        total += k;
    }
    return total;
}

// Розподіл рядків задає schedule(runtime) – див. omp_set_schedule.
long long Render(std::vector<double>& busy)
{
    long long total = 0;
    #pragma omp parallel reduction(+:total)
    {
        double start = omp_get_wtime();
        #pragma omp for schedule(runtime) nowait
        for (int y = 0; y < Height; ++y)
            total += Row(y);
        busy[omp_get_thread_num()] = omp_get_wtime() - start;
    }
    return total;
}

struct Mode { std::string name; omp_sched_t kind; int chunk; };

int main()
{
    int p = omp_get_max_threads();
    double start = omp_get_wtime();
    long long expected = 0;
    for (int y = 0; y < Height; ++y) expected += Row(y);
    double t1 = omp_get_wtime() - start;
    std::println("Послідовно: {:.3f} с, ітерацій {}", t1, expected);
    std::println("{:<12} {:>7} {:>6} {:>10}  {}",
                 "schedule", "T, с", "S", "Tmin/Tmax", "перевірка");

    std::vector<Mode> modes = {
        {"static", omp_sched_static, 0},
        {"static,8", omp_sched_static, 8},
        {"dynamic,1", omp_sched_dynamic, 1},
        {"guided", omp_sched_guided, 0}};
    for (const Mode& m : modes)
    {
        omp_set_schedule(m.kind, m.chunk);
        std::vector<double> busy(p);
        double t[3];
        long long total = 0;
        for (double& ti : t)
        {
            double s = omp_get_wtime();
            total = Render(busy);
            ti = omp_get_wtime() - s;
        }
        std::sort(t, t + 3);
        auto [lo, hi] = std::minmax_element(busy.begin(), busy.end());
        std::println("{:<12} {:>7.3f} {:>6.2f} {:>10.2f}  {}",
                     m.name, t[1], t1 / t[1], *lo / *hi,
                     total == expected ? "так" : "НІ");
    }
}
