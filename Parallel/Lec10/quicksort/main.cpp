#include <omp.h>
#include <algorithm>
#include <print>
#include <random>
#include <utility>
#include <vector>

// Розбиття Гоара навколо середнього елемента.
long long Partition(std::vector<int>& a, long long lo, long long hi)
{
    int pivot = a[lo + (hi - lo) / 2];
    long long i = lo - 1, j = hi + 1;
    while (true)
    {
        do ++i; while (a[i] < pivot);
        do --j; while (a[j] > pivot);
        if (i >= j) return j;
        std::swap(a[i], a[j]);
    }
}

// Задачі створюються лише для фрагментів, довших за поріг.
void QuickSort(std::vector<int>& a, long long lo, long long hi,
               long long cutoff)
{
    if (lo >= hi) return;
    long long mid = Partition(a, lo, hi);
    if (hi - lo > cutoff)
    {
        #pragma omp task shared(a)
        QuickSort(a, lo, mid, cutoff);
        #pragma omp task shared(a)
        QuickSort(a, mid + 1, hi, cutoff);
        #pragma omp taskwait
    }
    else
    {
        QuickSort(a, lo, mid, cutoff);
        QuickSort(a, mid + 1, hi, cutoff);
    }
}

double SortTime(std::vector<int> a, long long cutoff)
{
    double start = omp_get_wtime();
    #pragma omp parallel
    #pragma omp single
    QuickSort(a, 0, (long long)a.size() - 1, cutoff);
    double time = omp_get_wtime() - start;
    if (!std::is_sorted(a.begin(), a.end()))
        std::println(stderr, "Помилка: масив не впорядковано");
    return time;
}

int main()
{
    const int N = 50'000'000;
    std::mt19937 gen(2026);
    std::vector<int> data(N);
    for (int& x : data) x = (int)gen();

    std::vector<int> copy = data;
    double start = omp_get_wtime();
    std::sort(copy.begin(), copy.end());
    double tStd = omp_get_wtime() - start;
    std::println("N = {}, потоків: {}", N, omp_get_max_threads());
    std::println("std::sort: {:.3f} с", tStd);

    double t0 = SortTime(data, N);        // без задач
    std::println("{:>10} {:>8} {:>7}", "поріг", "T, с", "S");
    for (long long cutoff : {N, 1'000'000, 100'000, 10'000,
                             1'000, 100, 10})
    {
        double t = cutoff == N ? t0 : SortTime(data, cutoff);
        std::println("{:>10} {:>8.3f} {:>7.2f}", cutoff, t, t0 / t);
    }
}
