cmake_minimum_required(VERSION 3.30)
project(Bank LANGUAGES CXX)

find_package(Threads REQUIRED)

add_executable(bank main.cpp)
target_compile_features(bank PRIVATE cxx_std_23)
target_compile_options(bank PRIVATE
    -Wall -Wextra "$<$<CONFIG:Release>:-O3;-march=native>")
target_link_libraries(bank PRIVATE Threads::Threads)

# std::print у MinGW (Windows) потребує додаткової бібліотеки.
if(MINGW)
    target_link_libraries(bank PRIVATE stdc++exp)
endif()
