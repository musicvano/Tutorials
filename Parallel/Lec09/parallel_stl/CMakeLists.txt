cmake_minimum_required(VERSION 3.30)
project(ParallelStl LANGUAGES CXX)

add_executable(parallel_stl main.cpp)
target_compile_features(parallel_stl PRIVATE cxx_std_23)

# libstdc++ (GCC) виконує par і par_unseq через бібліотеку TBB.
if(CMAKE_CXX_COMPILER_ID STREQUAL "GNU")
    find_package(TBB QUIET)
    if(TBB_FOUND)
        target_link_libraries(parallel_stl PRIVATE TBB::tbb)
    else()
        message(WARNING "TBB не знайдено: par працює послідовно")
    endif()
    target_compile_options(parallel_stl PRIVATE
        -Wall -Wextra "$<$<CONFIG:Release>:-O3;-march=native>")
endif()

if(MSVC)
    target_compile_options(parallel_stl PRIVATE /O2 /utf-8)
endif()

# std::print у MinGW (Windows) потребує додаткової бібліотеки.
if(MINGW)
    target_link_libraries(parallel_stl PRIVATE stdc++exp)
endif()
