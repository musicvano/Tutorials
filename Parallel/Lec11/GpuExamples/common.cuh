// common.cuh – перевірка помилок CUDA і вимірювання часу.
#pragma once
#include <algorithm>
#include <chrono>
#include <cstdio>
#include <cstdlib>
#include <vector>
#include <cuda_runtime.h>

// Виклик CUDA Runtime API: у разі помилки – повідомлення й вихід.
#define CUDA_CHECK(call)                                          \
    do {                                                          \
        cudaError_t err = (call);                                 \
        if (err != cudaSuccess) {                                 \
            std::fprintf(stderr, "CUDA: %s (%s) у %s:%d\n",       \
                         cudaGetErrorString(err),                 \
                         cudaGetErrorName(err), __FILE__,         \
                         __LINE__);                               \
            std::exit(EXIT_FAILURE);                              \
        }                                                         \
    } while (0)

// Медіана 5 вимірювань після прогрівання; run() повертає мс.
template <class F> float MedianMs(F run)
{
    run();
    std::vector<float> t;
    for (int r = 0; r < 5; ++r) t.push_back(run());
    std::sort(t.begin(), t.end());
    return t[2];
}

// Час фрагмента коду на CPU, мс.
template <class F> float CpuMs(F body)
{
    auto start = std::chrono::steady_clock::now();
    body();
    std::chrono::duration<float, std::milli> d =
        std::chrono::steady_clock::now() - start;
    return d.count();
}

// Час фрагмента на GPU за подіями CUDA, мс.
template <class F> float GpuMs(F body)
{
    cudaEvent_t start, stop;
    CUDA_CHECK(cudaEventCreate(&start));
    CUDA_CHECK(cudaEventCreate(&stop));
    CUDA_CHECK(cudaEventRecord(start));      // мітка в черзі GPU
    body();
    CUDA_CHECK(cudaEventRecord(stop));
    CUDA_CHECK(cudaEventSynchronize(stop));  // чекати на мітку stop
    float ms = 0;
    CUDA_CHECK(cudaEventElapsedTime(&ms, start, stop));
    CUDA_CHECK(cudaEventDestroy(start));
    CUDA_CHECK(cudaEventDestroy(stop));
    return ms;
}
