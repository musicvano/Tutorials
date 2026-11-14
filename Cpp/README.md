# Object-oriented programming in C++

Code examples for the course [Object-oriented programming in C++](https://mvano.com/courses/cpp/).
Each `LecNN` folder belongs to topic NN of the course.

## Tools

Visual Studio 2026 with the *Desktop development with C++* workload. Every project is set to
`Debug|x64`, `/std:c++latest`, `/utf-8` and `/W4`.

Open `LecNN\LecNN.slnx` in Visual Studio, or build a solution from the command line:

```powershell
& "${env:ProgramFiles}\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" `
    "Lec09\Lec09.slnx" -p:Configuration=Debug -p:Platform=x64
```

## Topics

| Folder            | Topic                                                                                   | Projects                                                                                                                                  |
| ----------------- | --------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- |
| [`Lec01`](Lec01)  | The C++ language and the C++26 standard, Visual Studio 2026, your first program, Git    | `Hello` (`#include <print>`), `HelloModules` (the same program with `import std;`), `Card` (`{:<28}` alignment), `Room` (`{:.2f}` arithmetic) |
| [`Lec02`](Lec02)  | Data types, variables, operators, console input and output (`std::print`), control flow | `Types`, `BMI` (input and formatting), `Narrowing` (narrowing conversions)                                                                 |
| [`Lec03`](Lec03)  | Functions: parameters, passing by reference, overloading, recursion                     | `Geometry` (overloads), `Hanoi` (recursion)                                                                                                |
| [`Lec04`](Lec04)  | Arrays, `std::string`, `std::string_view`, `std::vector`                                | `Vector`, `Bounds` (indexing and `at()`)                                                                                                   |
| [`Lec05`](Lec05)  | Pointers, references and memory management, smart pointers                              | `Memory`, `Playlist` (ownership), `Leaks`, `UseAfterFree` (the two classic faults)                                                        |
| [`Lec06`](Lec06)  | Debugging and handling errors: exceptions and `std::expected`                           | `Average` (a wrong result to step through), `TraceLoop` (tracepoints), `Unhandled` (an unhandled exception)                               |
| [`Lec07`](Lec07)  | Classes and objects: fields, methods, constructors, destructors, encapsulation          | `Account`                                                                                                                                  |
| [`Lec08`](Lec08)  | Copying and moving objects, RAII, the rules of zero, three and five                     | `Copies` (`Risky` vs `Safe`), `Timer` (`ScopeTimer`, RAII)                                                                                 |
| [`Lec09`](Lec09)  | Operator overloading and friend functions                                               | `Fraction`, `Money`                                                                                                                        |
| [`Lec10`](Lec10)  | Inheritance, virtual functions and polymorphism                                         | `Shapes`                                                                                                                                   |
| [`Lec11`](Lec11)  | Abstract classes, interfaces and multiple inheritance                                   | `Drawable` (interfaces), `Lamp` (several interfaces), `Diamond` (virtual inheritance)                                                     |
| [`Lec12`](Lec12)  | Function and class templates, concepts                                                  | `Plain` (the non-generic starting point), `Stack` (`FixedStack<T>`)                                                                       |
| [`Lec13`](Lec13)  | Standard library containers                                                             | `Contacts` (`map` / `unordered_map`), `Words` (counting)                                                                                   |
| [`Lec14`](Lec14)  | Iterators, algorithms, ranges and lambda expressions                                    | `Sales` (algorithms and ranges), `Invalidation` (iterator invalidation)                                                                   |
| [`Lec15`](Lec15)  | Input/output streams, text and binary files, `std::filesystem`                          | `Csv` (text), `Accounts` (binary), `Directory` (`std::filesystem`)                                                                        |
| [`Lec16`](Lec16)  | C++ modules, multi-file projects and new features of C++26                              | `Geometry` (`.ixx` modules), `fees` + `App` (a static library and its user), `LinkError` (a deliberate LNK2019), `Score` (CMake, modules, CTest) |

## Notes

- `Lec16\Score` is a CMake project: open its folder in Visual Studio (*File > Open > Folder*), not
  through `Lec16.slnx`.
- The `Lec15` examples create their own demo data (`grades-demo.csv`, `accounts-demo.dat`,
  `directory-demo\`) on each run, in the solution folder (the debugger working directory is set in
  each `.vcxproj`).
- Build output (`x64`, `obj`, `.vs`, `out`) is ignored by `.gitignore`.
