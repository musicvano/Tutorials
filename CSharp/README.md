# Object-oriented programming in C#

Code examples for the course [Object-oriented programming in C#](https://mvano.com/courses/csharp/).
Each topic of the course has its own folder named after the topic; the table below lists them in
course order and links each topic page.

## Tools

.NET 10 SDK and Visual Studio 2026 (JetBrains Rider or VS Code with C# Dev Kit work too).
Open `<Topic>\<Topic>.slnx`, or build and run from the command line:

```powershell
dotnet build Inheritance\Inheritance.slnx
dotnet run --project Inheritance\Payroll
dotnet test Testing\Testing.slnx
```

## Topics

Each solution holds the lecture examples and, in its `Practice` solution folder, one project per worked example of the topic's practice page (linked from that page).

| #                                                                  | Folder                           | Topic                                                            | Projects                                                                                                                                                                                                                                         |
| ------------------------------------------------------------------ | -------------------------------- | ---------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| [1](https://mvano.com/courses/csharp/01-dotnet-intro/)             | [`Intro`](Intro)                 | The .NET platform, development environment, C# program structure | `Hello` (top-level statements and the classic `Main`), `Temperature` (°C → °F/K with a retry loop), `Receipt` (`decimal`, VAT, aligned output), `Travel` (integer division, `DateTime.AddMinutes`); practice: `Greeting`, `Rectangle`, `AppInfo` |
| [2](https://mvano.com/courses/csharp/02-types-operators/)          | [`Types`](Types)                 | Data types, variables, operators, and expressions                | `Types` (parsing, `checked`, overflow); practice: `SpeedUnits`, `TaxiFare`, `BitAnalysis`                                                                                                                                                        |
| [3](https://mvano.com/courses/csharp/03-control-flow/)             | [`ControlFlow`](ControlFlow)     | Control flow: branching and loops                                | `Control`; practice: `Seasons`, `PrimeRange`, `NewtonSqrt`                                                                                                                                                                                       |
| [4](https://mvano.com/courses/csharp/04-arrays-strings/)           | [`ArraysStrings`](ArraysStrings) | Arrays and strings                                               | `ArraysStrings` (arrays, `StringBuilder`, `Stopwatch`); practice: `ArrayRotation`, `PascalTriangle`, `PhoneNumbers`                                                                                                                              |
| [5](https://mvano.com/courses/csharp/05-methods/)                  | [`Methods`](Methods)             | Methods: parameters, overloading, and recursion                  | `Methods` (parameters, overloads, XML doc comments); practice: `SwapSort`, `FormatOverloads`, `DigitSum`                                                                                                                                         |
| [6](https://mvano.com/courses/csharp/06-debugging-exceptions/)     | [`Exceptions`](Exceptions)       | Debugging and exception handling                                 | `Debugging` (an unhandled exception); practice: `MaxSearch`, `FractionCalculator`, `ConsoleMenu`                                                                                                                                                 |
| [7](https://mvano.com/courses/csharp/07-classes/)                  | [`Classes`](Classes)             | Classes and objects: fields, properties, and constructors        | `Banking` (`BankAccount`); practice: `Products`, `Rectangle`, `LapTimer`                                                                                                                                                                         |
| [8](https://mvano.com/courses/csharp/08-encapsulation-static/)     | [`Encapsulation`](Encapsulation) | Encapsulation, access modifiers, and static class members        | `Bank.Core` (`Account`, `Validator`, `InterestService`) + `Bank.App`; practice: `Temperature`, `Student`, `Common.Input`, `Survey.App`                                                                                                           |
| [9](https://mvano.com/courses/csharp/09-inheritance-polymorphism/) | [`Inheritance`](Inheritance)     | Inheritance and polymorphism                                     | `Payroll` (`Employee`, `SalariedEmployee`, `HourlyEmployee`); practice: `Animals`, `OverrideHiding`, `Messages`                                                                                                                                  |
| [10](https://mvano.com/courses/csharp/10-abstract-interfaces/)     | [`Interfaces`](Interfaces)       | Abstract classes and interfaces                                  | `Shapes` (`Shape` / `Circle`, `Switchable` / `SmartLamp`); practice: `InterestAccount`, `ExplicitInterfaces`, `ManagedResource`                                                                                                                  |
| [11](https://mvano.com/courses/csharp/11-structs-records/)         | [`Records`](Records)             | Structures, records, enumerations, and tuples                    | `ValueTypes` (tuples, `record`, `struct`); practice: `ClosestPair`, `SeasonParsing`, `MeasurementStats`                                                                                                                                          |
| [12](https://mvano.com/courses/csharp/12-operators-indexers/)      | [`Operators`](Operators)         | Operator overloading, indexers, and extension methods            | `Operators` (`Fraction`, `Matrix`, `ChessBoard`, `StringExtensions`); practice: `PlaneVector`, `Polynomial`, `IntExtensions`                                                                                                                     |
| [13](https://mvano.com/courses/csharp/13-generics-collections/)    | [`Generics`](Generics)           | Generics and .NET collections                                    | `Collections` (`Repository<T>`, collection pitfalls); practice: `ValueRange`, `Brackets`, `LookupSpeed`                                                                                                                                          |
| [14](https://mvano.com/courses/csharp/14-delegates-events/)        | [`Delegates`](Delegates)         | Delegates, lambda expressions, and events                        | `Delegates` (`Thermostat` and its event); practice: `DelegateCalculator`, `Retries`, `AccountEvents`                                                                                                                                             |
| [15](https://mvano.com/courses/csharp/15-linq/)                    | [`Linq`](Linq)                   | Language-Integrated Query (LINQ)                                 | `Linq` (queries over `Student`); practice: `WordAnalysis`, `Pagination`, `CustomOperators`                                                                                                                                                       |
| [16](https://mvano.com/courses/csharp/16-files-json/)              | [`Files`](Files)                 | Files, streams, and JSON serialization                           | `FilesDemo` (`grades.csv` input, JSON round-trip); practice: `DirectoryReport`, `HighScores`, `AppSettings`                                                                                                                                      |
| [17](https://mvano.com/courses/csharp/17-solid-patterns/)          | [`Patterns`](Patterns)           | SOLID principles and design patterns                             | `Patterns` (dependency injection, SOLID refactorings); practice: `LiskovFix`, `ExchangeAdapter`, `UndoRedo`                                                                                                                                      |
| [18](https://mvano.com/courses/csharp/18-testing-refactoring/)     | [`Testing`](Testing)             | Unit testing and code refactoring                                | `Shop.Core` (the code to refactor) + `Shop.Tests` (MSTest, `FakeTimeProvider`)                                                                                                                                                                   |

## Notes

- `*.complete.txt` (`Inheritance\Payroll`, `Interfaces\Shapes`) and `Files\FilesDemo\JsonExample.cs.txt` are
  the finished versions of the lab tasks, kept next to the file they replace.
- `Files\FilesDemo\grades.csv` is deliberately dirty input data; the `data\` folder the example
  writes is generated on each run.
- `Encapsulation\.editorconfig` lowers the analyzer rule CA1822 to a suggestion.
- Build output (`bin`, `obj`, `.vs`, `TestResults`) is ignored by `.gitignore`.
