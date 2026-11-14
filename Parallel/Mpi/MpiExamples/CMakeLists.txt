cmake_minimum_required(VERSION 3.28)
project(MpiExamples LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
if(NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Release)
endif()

find_package(MPI REQUIRED)
find_package(OpenMP REQUIRED)

foreach(name hello pi ring pingpong jacobi)
    add_executable(${name} ${name}.cpp)
    target_link_libraries(${name} PRIVATE MPI::MPI_CXX)
endforeach()
# Гібридна програма: MPI між процесами, OpenMP усередині.
target_link_libraries(jacobi PRIVATE OpenMP::OpenMP_CXX)
