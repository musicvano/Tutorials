cmake_minimum_required(VERSION 3.28)
project(VectorAdd LANGUAGES CXX CUDA)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CUDA_STANDARD 20)
set(CMAKE_CUDA_ARCHITECTURES 86)      # RTX 3060, CC 8.6

find_package(CUDAToolkit REQUIRED)

add_executable(vector_add vector_add.cu)
target_link_libraries(vector_add PRIVATE CUDA::cudart)
