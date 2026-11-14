cmake_minimum_required(VERSION 3.28)
project(QuickSort LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

find_package(OpenMP REQUIRED)

add_executable(quicksort main.cpp)
target_link_libraries(quicksort PRIVATE OpenMP::OpenMP_CXX)

if(MINGW)
    # MinGW у Windows: CMake не додає libgomp під час компонування,
    # а std::print потребує бібліотеки libstdc++exp.
    target_link_options(quicksort PRIVATE -fopenmp)
    target_link_libraries(quicksort PRIVATE stdc++exp)
endif()
