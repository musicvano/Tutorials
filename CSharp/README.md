# Object-oriented programming in C#

Code examples for the course [Object-oriented programming in C#](https://mvano.com/courses/csharp/).
Each `LecNN` folder belongs to topic NN of the course.

## Tools

.NET 10 SDK and Visual Studio 2026 (JetBrains Rider or VS Code with C# Dev Kit work too).
Open `LecNN\LecNN.slnx`, or build and run from the command line:

```powershell
dotnet build Lec09\Lec09.slnx
dotnet run --project Lec09\Payroll
dotnet test Lec18\Lec18.slnx
```

## Topics

| Folder            | Topic                                                          | Projects                                                                                                                         |
| ----------------- | -------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------- |
| [`Lec01`](Lec01)  | The .NET platform, development environment, C# program structure | `Hello` (top-level statements and the classic `Main`), `Temperature` (°C → °F/K with a retry loop), `Receipt` (`decimal`, VAT, aligned output), `Travel` (integer division, `DateTime.AddMinutes`) |
| [`Lec02`](Lec02)  | Data types, variables, operators, and expressions              | `Types` (parsing, `checked`, overflow)                                                                                           |
| [`Lec03`](Lec03)  | Control flow: branching and loops                              | `Control`                                                                                                                        |
| [`Lec04`](Lec04)  | Arrays and strings                                             | `ArraysStrings` (arrays, `StringBuilder`, `Stopwatch`)                                                                           |
| [`Lec05`](Lec05)  | Methods: parameters, overloading, and recursion                | `Methods` (parameters, overloads, XML doc comments)                                                                              |
| [`Lec06`](Lec06)  | Debugging and exception handling                               | `Debugging` (an unhandled exception)                                                                                             |
| [`Lec07`](Lec07)  | Classes and objects: fields, properties, and constructors      | `Banking` (`BankAccount`)                                                                                                        |
| [`Lec08`](Lec08)  | Encapsulation, access modifiers, and static class members      | `Bank.Core` (`Account`, `Validator`, `InterestService`) + `Bank.App`                                                             |
| [`Lec09`](Lec09)  | Inheritance and polymorphism                                   | `Payroll` (`Employee`, `SalariedEmployee`, `HourlyEmployee`)                                                                     |
| [`Lec10`](Lec10)  | Abstract classes and interfaces                                | `Shapes` (`Shape` / `Circle`, `Switchable` / `SmartLamp`)                                                                        |
| [`Lec11`](Lec11)  | Structures, records, enumerations, and tuples                  | `ValueTypes` (tuples, `record`, `struct`)                                                                                        |
| [`Lec12`](Lec12)  | Operator overloading, indexers, and extension methods          | `Operators` (`Fraction`, `Matrix`, `ChessBoard`, `StringExtensions`)                                                             |
| [`Lec13`](Lec13)  | Generics and .NET collections                                  | `Collections` (`Repository<T>`, collection pitfalls)                                                                             |
| [`Lec14`](Lec14)  | Delegates, lambda expressions, and events                      | `Delegates` (`Thermostat` and its event)                                                                                         |
| [`Lec15`](Lec15)  | Language-Integrated Query (LINQ)                               | `Linq` (queries over `Student`)                                                                                                  |
| [`Lec16`](Lec16)  | Files, streams, and JSON serialization                         | `FilesDemo` (`grades.csv` input, JSON round-trip)                                                                                |
| [`Lec17`](Lec17)  | SOLID principles and design patterns                           | `Patterns` (dependency injection, SOLID refactorings)                                                                            |
| [`Lec18`](Lec18)  | Unit testing and code refactoring                              | `Shop.Core` (the code to refactor) + `Shop.Tests` (MSTest, `FakeTimeProvider`)                                                   |

## Notes

- `*.complete.txt` (`Lec09\Payroll`, `Lec10\Shapes`) and `Lec16\FilesDemo\JsonExample.cs.txt` are
  the finished versions of the lab tasks, kept next to the file they replace.
- `Lec16\FilesDemo\grades.csv` is deliberately dirty input data; the `data\` folder the example
  writes is generated on each run.
- `Lec08\.editorconfig` lowers the analyzer rule CA1822 to a suggestion.
- Build output (`bin`, `obj`, `.vs`, `TestResults`) is ignored by `.gitignore`.
