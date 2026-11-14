cmake_minimum_required(VERSION 3.31)
project(GpuExamples LANGUAGES CXX CUDA)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CUDA_STANDARD 20)
set(CMAKE_CUDA_ARCHITECTURES 86)

find_package(CUDAToolkit REQUIRED)
find_package(OpenMP REQUIRED)      # OpenMP::OpenMP_CUDA: CMake 3.31+

# Windows (MSVC): UTF-8 і стандартний препроцесор для CCCL (Thrust).
set(WIN_FLAGS -Xcompiler=/utf-8,/Zc:preprocessor)

foreach(name grayscale matmul)
    add_executable(${name} ${name}.cu)
    target_link_libraries(${name} PRIVATE CUDA::cudart
                          OpenMP::OpenMP_CUDA)
    if(MSVC)
        target_compile_options(${name} PRIVATE
            $<$<COMPILE_LANGUAGE:CUDA>:${WIN_FLAGS}>)
    endif()
endforeach()
