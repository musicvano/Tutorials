// vector_add.cu – сума векторів c = a + b на GPU.
#include <cmath>
#include <cstdio>
#include <vector>
#include "common.cuh"

// Ядро: кожен потік обчислює один елемент.
__global__ void VectorAdd(const float* a, const float* b, float* c,
                          int n)
{
    int i = blockIdx.x * blockDim.x + threadIdx.x;  // індекс потоку
    if (i < n)                                       // перевірка меж
        c[i] = a[i] + b[i];
}

int main()
{
    cudaDeviceProp prop;
    CUDA_CHECK(cudaGetDeviceProperties(&prop, 0));
    std::printf("GPU: %s, CC %d.%d, SM: %d, "
                "пам'ять: %.1f ГБ\n", prop.name, prop.major,
                prop.minor, prop.multiProcessorCount,
                prop.totalGlobalMem / 1073741824.0);

    const int n = 50'000'000;
    const size_t bytes = n * sizeof(float);
    std::vector<float> a(n), b(n), c(n);
    for (int i = 0; i < n; ++i)
    {
        a[i] = std::sin(i) * std::sin(i);
        b[i] = std::cos(i) * std::cos(i);
    }

    // 1. Пам'ять пристрою.
    float *dA, *dB, *dC;
    CUDA_CHECK(cudaMalloc(&dA, bytes));
    CUDA_CHECK(cudaMalloc(&dB, bytes));
    CUDA_CHECK(cudaMalloc(&dC, bytes));

    // 2. Копіювання хост -> пристрій.
    CUDA_CHECK(cudaMemcpy(dA, a.data(), bytes,
                          cudaMemcpyHostToDevice));
    CUDA_CHECK(cudaMemcpy(dB, b.data(), bytes,
                          cudaMemcpyHostToDevice));

    // 3. Запуск ядра: блоки по 256 потоків.
    int threads = 256;
    int blocks = (n + threads - 1) / threads;
    VectorAdd<<<blocks, threads>>>(dA, dB, dC, n);
    CUDA_CHECK(cudaGetLastError());          // помилка запуску
    CUDA_CHECK(cudaDeviceSynchronize());     // помилка виконання

    // 4. Копіювання пристрій -> хост (чекає на завершення ядра).
    CUDA_CHECK(cudaMemcpy(c.data(), dC, bytes,
                          cudaMemcpyDeviceToHost));

    // 5. Звільнення пам'яті пристрою.
    CUDA_CHECK(cudaFree(dA));
    CUDA_CHECK(cudaFree(dB));
    CUDA_CHECK(cudaFree(dC));

    // sin² + cos² = 1 для кожного елемента.
    float maxError = 0.0f;
    for (int i = 0; i < n; ++i)
        maxError = std::fmax(maxError, std::fabs(c[i] - 1.0f));
    std::printf("n = %d, блоків: %d, потоків у блоці: %d\n",
                n, blocks, threads);
    std::printf("Max error: %g\n", maxError);
}
