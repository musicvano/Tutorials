// grayscale.cu – відтінки сірого для кольорового зображення на
// 2D-сітці потоків. Аргументи: ширина висота (типово 3840 2160).
#include <omp.h>
#include <cstdlib>
#include <fstream>
#include "common.cuh"

struct Rgb { unsigned char r, g, b; };

// Потік (x, y) обчислює яскравість одного пікселя.
__global__ void Grayscale(const Rgb* in, unsigned char* out,
                          int width, int height)
{
    int x = blockIdx.x * blockDim.x + threadIdx.x;   // стовпець
    int y = blockIdx.y * blockDim.y + threadIdx.y;   // рядок
    if (x < width && y < height)
    {
        Rgb p = in[y * width + x];
        out[y * width + x] = (unsigned char)(
            0.299f * p.r + 0.587f * p.g + 0.114f * p.b + 0.5f);
    }
}

// Та сама формула на CPU: threads = 1 – послідовно, інакше OpenMP.
void GrayscaleCpu(const std::vector<Rgb>& in,
                  std::vector<unsigned char>& out, int threads)
{
    const long long n = (long long)in.size();
    #pragma omp parallel for num_threads(threads)
    for (long long i = 0; i < n; ++i)
        out[i] = (unsigned char)(0.299f * in[i].r
            + 0.587f * in[i].g + 0.114f * in[i].b + 0.5f);
}

int main(int argc, char* argv[])
{
    const int w = argc > 1 ? std::atoi(argv[1]) : 3840;
    const int h = argc > 2 ? std::atoi(argv[2]) : 2160;
    const size_t n = (size_t)w * h;
    std::vector<Rgb> image(n);                   // градієнти
    for (int y = 0; y < h; ++y)
        for (int x = 0; x < w; ++x)
            image[(size_t)y * w + x] = {(unsigned char)(x * 255 / w),
                (unsigned char)(y * 255 / h), (unsigned char)(x ^ y)};
    std::vector<unsigned char> gray(n), check(n);

    Rgb* dIn;
    unsigned char* dOut;
    CUDA_CHECK(cudaMalloc(&dIn, n * sizeof(Rgb)));
    CUDA_CHECK(cudaMalloc(&dOut, n));
    dim3 block(16, 16);                          // 256 потоків
    dim3 grid((w + block.x - 1) / block.x,
              (h + block.y - 1) / block.y);

    float h2d = MedianMs([&] { return GpuMs([&] {
        CUDA_CHECK(cudaMemcpy(dIn, image.data(), n * sizeof(Rgb),
                              cudaMemcpyHostToDevice)); }); });
    float kernel = MedianMs([&] { return GpuMs([&] {
        Grayscale<<<grid, block>>>(dIn, dOut, w, h);
        CUDA_CHECK(cudaGetLastError()); }); });
    float d2h = MedianMs([&] { return GpuMs([&] {
        CUDA_CHECK(cudaMemcpy(gray.data(), dOut, n,
                              cudaMemcpyDeviceToHost)); }); });
    float cpu1 = MedianMs([&] {
        return CpuMs([&] { GrayscaleCpu(image, check, 1); }); });
    int p = omp_get_num_procs();
    float cpuP = MedianMs([&] {
        return CpuMs([&] { GrayscaleCpu(image, check, p); }); });

    int maxDiff = 0;
    for (size_t i = 0; i < n; ++i)
        maxDiff = std::max(maxDiff, std::abs(gray[i] - check[i]));
    std::ofstream file("gray.pgm", std::ios::binary);   // формат PGM
    file << "P5\n" << w << ' ' << h << "\n255\n";
    file.write((const char*)gray.data(), n);

    float total = h2d + kernel + d2h;
    std::printf("Зображення %dx%d, сітка %ux%u блоків 16x16\n",
                w, h, grid.x, grid.y);
    std::printf("GPU: H2D %.2f + ядро %.3f + D2H %.2f = %.2f мс\n",
                h2d, kernel, d2h, total);
    std::printf("CPU: 1 потік %.2f мс, OpenMP (%d) %.2f мс\n",
                cpu1, p, cpuP);
    std::printf("Прискорення відносно OpenMP: ядро %.1f, "
                "з копіюваннями %.2f\n", cpuP / kernel, cpuP / total);
    std::printf("Найбільша різниця з CPU: %d\n", maxDiff);
    CUDA_CHECK(cudaFree(dIn));
    CUDA_CHECK(cudaFree(dOut));
}
