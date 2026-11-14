cmake_minimum_required(VERSION 3.30)
project(ThreadPool LANGUAGES CXX)

find_package(Threads REQUIRED)

add_executable(pool main.cpp)
target_compile_features(pool PRIVATE cxx_std_23)
target_compile_options(pool PRIVATE
    -Wall -Wextra "$<$<CONFIG:Release>:-O3;-march=native>")
target_link_libraries(pool PRIVATE Threads::Threads)

# std::print у MinGW (Windows) потребує додаткової бібліотеки.
if(MINGW)
    target_link_libraries(pool PRIVATE stdc++exp)
endif()
