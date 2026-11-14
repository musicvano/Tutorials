cmake_minimum_required(VERSION 3.30)
project(ParallelSum LANGUAGES CXX)

find_package(Threads REQUIRED)

add_executable(parallel_sum main.cpp)
target_compile_features(parallel_sum PRIVATE cxx_std_23)
target_compile_options(parallel_sum PRIVATE
    -Wall -Wextra "$<$<CONFIG:Release>:-O3;-march=native>")
target_link_libraries(parallel_sum PRIVATE Threads::Threads)

# std::print у MinGW (Windows) потребує додаткової бібліотеки.
if(MINGW)
    target_link_libraries(parallel_sum PRIVATE stdc++exp)
endif()
