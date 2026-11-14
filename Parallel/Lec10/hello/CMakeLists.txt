cmake_minimum_required(VERSION 3.28)
project(HelloOpenMP LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

find_package(OpenMP REQUIRED)

add_executable(hello main.cpp)
target_link_libraries(hello PRIVATE OpenMP::OpenMP_CXX)

if(MINGW)
    # MinGW у Windows: CMake не додає libgomp під час компонування,
    # а std::print потребує бібліотеки libstdc++exp.
    target_link_options(hello PRIVATE -fopenmp)
    target_link_libraries(hello PRIVATE stdc++exp)
endif()
