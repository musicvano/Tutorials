// matmul.cu – множення матриць C = A·B: CPU (OpenMP), наївне ядро
// та ядро з тайлами у спільній пам'яті. Аргумент: n (кратне 32).
#include <omp.h>
#include <cmath>
#include <random>
#include "common.cuh"

// Наївне ядро: кожен потік читає рядок A і стовпець B
// з глобальної пам'яті.
__global__ void MatMulNaive(const float* a, const float* b, float* c,
                            int n)
{
    int row = blockIdx.y * blockDim.y + threadIdx.y;
    int col = blockIdx.x * blockDim.x + threadIdx.x;
    if (row < n && col < n)
    {
        float sum = 0.0f;
        for (int k = 0; k < n; ++k)
            sum += a[row * n + k] * b[k * n + col];
        c[row * n + col] = sum;
    }
}

// Ядро з тайлами: блок Tile×Tile потоків по черзі завантажує
// тайли A і B у спільну пам'ять (n кратне Tile).
template <int Tile>
__global__ void MatMulTiled(const float* a, const float* b, float* c,
                            int n)
{
    __shared__ float tileA[Tile][Tile];
    __shared__ float tileB[Tile][Tile];
    int tx = threadIdx.x, ty = threadIdx.y;
    int row = blockIdx.y * Tile + ty;
    int col = blockIdx.x * Tile + tx;
    float sum = 0.0f;
    for (int phase = 0; phase < n / Tile; ++phase)
    {
        // Кожен потік копіює по одному елементу A і B.
        tileA[ty][tx] = a[row * n + phase * Tile + tx];
        tileB[ty][tx] = b[(phase * Tile + ty) * n + col];
        __syncthreads();                 // тайли завантажено
        for (int k = 0; k < Tile; ++k)
            sum += tileA[ty][k] * tileB[k][tx];
        __syncthreads();                 // тайли вже не потрібні
    }
    c[row * n + col] = sum;
}

// CPU: рядки C між потоками OpenMP, порядок циклів i-k-j.
void MatMulCpu(const std::vector<float>& a,
               const std::vector<float>& b, std::vector<float>& c,
               int n)
{
    #pragma omp parallel for
    for (int i = 0; i < n; ++i)
    {
        float* ci = &c[(size_t)i * n];
        std::fill(ci, ci + n, 0.0f);
        for (int k = 0; k < n; ++k)
        {
            float aik = a[(size_t)i * n + k];
            const float* bk = &b[(size_t)k * n];
            for (int j = 0; j < n; ++j) ci[j] += aik * bk[j];
        }
    }
}

int main(int argc, char* argv[])
{
    const int n = argc > 1 ? std::atoi(argv[1]) : 2048;
    if (n <= 0 || n % 32 != 0)
    {
        std::fprintf(stderr, "n має бути додатним і кратним 32\n");
        return 1;
    }
    const size_t bytes = (size_t)n * n * sizeof(float);
    std::mt19937 gen(10);
    std::uniform_real_distribution<float> dist(-1.0f, 1.0f);
    std::vector<float> a((size_t)n * n), b(a.size()), c(a.size()),
        ref(a.size());
    for (float& x : a) x = dist(gen);
    for (float& x : b) x = dist(gen);

    float *dA, *dB, *dC;
    CUDA_CHECK(cudaMalloc(&dA, bytes));
    CUDA_CHECK(cudaMalloc(&dB, bytes));
    CUDA_CHECK(cudaMalloc(&dC, bytes));
    float copyMs = MedianMs([&] { return GpuMs([&] {
        CUDA_CHECK(cudaMemcpy(dA, a.data(), bytes,
                              cudaMemcpyHostToDevice));
        CUDA_CHECK(cudaMemcpy(dB, b.data(), bytes,
                              cudaMemcpyHostToDevice));
        CUDA_CHECK(cudaMemcpy(c.data(), dC, bytes,
                              cudaMemcpyDeviceToHost)); }); });
    float cpuMs = MedianMs([&] {
        return CpuMs([&] { MatMulCpu(a, b, ref, n); }); });

    const double flop = 2.0 * n * n * n;
    std::printf("n = %d, CPU: %d потоків OpenMP\n", n,
                omp_get_max_threads());
    std::printf(" час, мс  GFLOPS       S   похибка  спосіб\n");
    std::printf("%8.2f %7.1f %7.1f %9s  CPU, OpenMP\n", cpuMs,
                flop / cpuMs / 1e6, 1.0, "-");
    // Запуск ядра: медіана часу, перевірка з результатом CPU.
    auto run = [&](const char* name, auto kernel, int tile) {
        dim3 block(tile, tile), grid(n / tile, n / tile);
        float ms = MedianMs([&] { return GpuMs([&] {
            kernel<<<grid, block>>>(dA, dB, dC, n);
            CUDA_CHECK(cudaGetLastError()); }); });
        CUDA_CHECK(cudaMemcpy(c.data(), dC, bytes,
                              cudaMemcpyDeviceToHost));
        float err = 0.0f;
        for (size_t i = 0; i < c.size(); ++i)
            err = std::max(err, std::fabs(c[i] - ref[i]));
        std::printf("%8.2f %7.1f %7.1f %9.1e  %s\n", ms,
                    flop / ms / 1e6, cpuMs / ms, err, name);
    };
    run("GPU, наївне ядро 16x16", MatMulNaive, 16);
    run("GPU, тайли 16x16", MatMulTiled<16>, 16);
    run("GPU, тайли 32x32", MatMulTiled<32>, 32);
    std::printf("Копіювання A, B на GPU і C назад: %.2f мс\n",
                copyMs);
    CUDA_CHECK(cudaFree(dA));
    CUDA_CHECK(cudaFree(dB));
    CUDA_CHECK(cudaFree(dC));
}
