# Object-oriented programming in C++

Code examples for the course [Object-oriented programming in C++](https://mvano.com/courses/cpp/).
Each topic of the course has its own folder named after the topic; the table below lists them in
course order and links each topic page.

## Tools

Visual Studio 2026 with the *Desktop development with C++* workload. Every project is set to
`Debug|x64`, `/std:c++latest`, `/utf-8` and `/W4`.

Open `<Topic>\<Topic>.slnx` in Visual Studio, or build a solution from the command line:

```powershell
& "${env:ProgramFiles}\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" `
    "Operators\Operators.slnx" -p:Configuration=Debug -p:Platform=x64
```

## Topics

| #                                                                   | Folder                           | Topic                                                                                   | Projects                                                                                                                                         |
| ------------------------------------------------------------------- | -------------------------------- | --------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------ |
| [1](https://mvano.com/courses/cpp/01-intro/)                        | [`Intro`](Intro)                 | The C++ language and the C++26 standard, Visual Studio 2026, your first program, Git    | `Hello` (`#include <print>`), `HelloModules` (the same program with `import std;`), `Card` (`{:<28}` alignment), `Room` (`{:.2f}` arithmetic)    |
| [2](https://mvano.com/courses/cpp/02-types-control-flow/)           | [`ControlFlow`](ControlFlow)     | Data types, variables, operators, console input and output (`std::print`), control flow | `Types`, `BMI` (input and formatting), `Narrowing` (narrowing conversions)                                                                       |
| [3](https://mvano.com/courses/cpp/03-functions/)                    | [`Functions`](Functions)         | Functions: parameters, passing by reference, overloading, recursion                     | `Geometry` (overloads), `Hanoi` (recursion)                                                                                                      |
| [4](https://mvano.com/courses/cpp/04-arrays-strings/)               | [`ArraysStrings`](ArraysStrings) | Arrays, `std::string`, `std::string_view`, `std::vector`                                | `Vector`, `Bounds` (indexing and `at()`)                                                                                                         |
| [5](https://mvano.com/courses/cpp/05-pointers-memory/)              | [`Pointers`](Pointers)           | Pointers, references and memory management, smart pointers                              | `Memory`, `Playlist` (ownership), `Leaks`, `UseAfterFree` (the two classic faults)                                                               |
| [6](https://mvano.com/courses/cpp/06-errors-debugging/)             | [`Debugging`](Debugging)         | Debugging and handling errors: exceptions and `std::expected`                           | `Average` (a wrong result to step through), `TraceLoop` (tracepoints), `Unhandled` (an unhandled exception)                                      |
| [7](https://mvano.com/courses/cpp/07-classes/)                      | [`Classes`](Classes)             | Classes and objects: fields, methods, constructors, destructors, encapsulation          | `Account`                                                                                                                                        |
| [8](https://mvano.com/courses/cpp/08-copy-move-raii/)               | [`CopyMove`](CopyMove)           | Copying and moving objects, RAII, the rules of zero, three and five                     | `Copies` (`Risky` vs `Safe`), `Timer` (`ScopeTimer`, RAII)                                                                                       |
| [9](https://mvano.com/courses/cpp/09-operators/)                    | [`Operators`](Operators)         | Operator overloading and friend functions                                               | `Fraction`, `Money`                                                                                                                              |
| [10](https://mvano.com/courses/cpp/10-inheritance-polymorphism/)    | [`Inheritance`](Inheritance)     | Inheritance, virtual functions and polymorphism                                         | `Shapes`                                                                                                                                         |
| [11](https://mvano.com/courses/cpp/11-abstract-interfaces/)         | [`Interfaces`](Interfaces)       | Abstract classes, interfaces and multiple inheritance                                   | `Drawable` (interfaces), `Lamp` (several interfaces), `Diamond` (virtual inheritance)                                                            |
| [12](https://mvano.com/courses/cpp/12-templates-concepts/)          | [`Templates`](Templates)         | Function and class templates, concepts                                                  | `Plain` (the non-generic starting point), `Stack` (`FixedStack<T>`)                                                                              |
| [13](https://mvano.com/courses/cpp/13-containers/)                  | [`Containers`](Containers)       | Standard library containers                                                             | `Contacts` (`map` / `unordered_map`), `Words` (counting)                                                                                         |
| [14](https://mvano.com/courses/cpp/14-iterators-algorithms-ranges/) | [`Algorithms`](Algorithms)       | Iterators, algorithms, ranges and lambda expressions                                    | `Sales` (algorithms and ranges), `Invalidation` (iterator invalidation)                                                                          |
| [15](https://mvano.com/courses/cpp/15-io-files/)                    | [`Files`](Files)                 | Input/output streams, text and binary files, `std::filesystem`                          | `Csv` (text), `Accounts` (binary), `Directory` (`std::filesystem`)                                                                               |
| [16](https://mvano.com/courses/cpp/16-modules-cpp26/)               | [`Modules`](Modules)             | C++ modules, multi-file projects and new features of C++26                              | `Geometry` (`.ixx` modules), `fees` + `App` (a static library and its user), `LinkError` (a deliberate LNK2019), `Score` (CMake, modules, CTest) |

## Notes

- `Modules\Score` is a CMake project: open its folder in Visual Studio (*File > Open > Folder*), not
  through `Modules.slnx`.
- The `Files` examples create their own demo data (`grades-demo.csv`, `accounts-demo.dat`,
  `directory-demo\`) on each run, in the solution folder (the debugger working directory is set in
  each `.vcxproj`).
- Build output (`x64`, `obj`, `.vs`, `out`) is ignored by `.gitignore`.
