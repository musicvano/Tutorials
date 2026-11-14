# Object-oriented programming in C# (GUI)

Code examples for the course [Object-oriented programming in C# (GUI)](https://mvano.com/courses/csharp-gui/):
desktop, web, network, database and AI applications in .NET. Each `LecNN` folder belongs to topic
NN of the course. Topic 1 (Git) has no code folder: its lecture builds the demo repositories step by
step.

## Tools

- .NET 10 SDK and Visual Studio 2026 on Windows (Windows Forms and WPF projects target
  `net10.0-windows`).
- Topic 15: the .NET MAUI workload (`dotnet workload install maui`) and an Android emulator or Windows.
- Topics 7–8: PostgreSQL 18.
- Topic 16: [Ollama](https://ollama.com/) with the models `qwen3:4b-instruct` and `embeddinggemma`.

Open `LecNN\LecNN.slnx`, or build and run from the command line:

```powershell
dotnet build Lec09\Lec09.slnx
dotnet run --project Lec09\EchoServer
```

## Topics

| Folder            | Topic                                                              | Projects                                                                                                                                                          |
| ----------------- | ------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| [`Lec02`](Lec02)  | Regular expressions and text processing                            | `Regexes` (date pattern, `dates.txt`), `Censor` (`[GeneratedRegex]`), `Contacts.Core` + `Contacts.Tests` (xUnit v3, one test fails on purpose)                   |
| [`Lec03`](Lec03)  | Windows Forms fundamentals                                         | `TemperatureConverter` (`ErrorProvider`), `LayoutDemo` (`TableLayoutPanel`), `TextEditor`, `BookCatalog`, `BmiCalculator`, `StopwatchApp`, `PhoneBook`             |
| [`Lec04`](Lec04)  | Graphics with Windows Forms and GDI+                               | `Graph` (y = sin(x)·x), `Clock` (analog clock), `Editor` (paint editor, Save PNG, Print Preview), `Gauge` (custom control)                                       |
| [`Lec05`](Lec05)  | Asynchronous programming with async/await in GUI applications      | `Primes`, `FileSearch`, `Pages`, `Checksums`, `Mandelbrot`, `Resize`; `Make-Data.ps1` creates their test data                                                    |
| [`Lec06`](Lec06)  | Dependency injection, configuration, and logging                   | `Notifications`, `ConfigDemo` (launch profile, user secrets), `WeatherMonitor` (Generic Host worker), `TodoApp` (Windows Forms + host)                           |
| [`Lec07`](Lec07)  | ADO.NET: relational databases and SQL                              | `Library`, `Injection` (SQL injection and parameters), `BookBrowser`; SQL scripts; `Setup-Db.ps1` creates the databases                                         |
| [`Lec08`](Lec08)  | Object-relational mapping with Entity Framework Core               | `Blog`, `Shop`, `States`, `Stock`                                                                                                                                 |
| [`Lec09`](Lec09)  | Network applications with `Socket`                                 | `EchoServer`, `EchoClient`, `ChatServer`, `ChatClient` (Windows Forms)                                                                                           |
| [`Lec10`](Lec10)  | REST web services: ASP.NET Core Minimal API, `HttpClient`, JSON    | `TodoApi` (ports 5080/7080, `TodoApi.http`), `TodoClient`                                                                                                        |
| [`Lec11`](Lec11)  | Real-time messaging with ASP.NET Core SignalR                      | `ChatServer`, `ChatClient`, `RoomsClient`, `BoardServer`, `BoardClient`                                                                                          |
| [`Lec12`](Lec12)  | Windows Presentation Foundation: XAML, layout, routed events       | `Registration`, `RegistrationFluent` (argument `dark`), `UnitConverter`, `EventDemo`, `FileManager`                                                              |
| [`Lec13`](Lec13)  | Data binding, styles, templates, and MVVM in WPF                   | `Profile`, `Catalog`, `Styles`, `Registration`, `TodoApp` + `TodoApp.Core` + `TodoApp.Tests`, `Schedule`, `Cards`, `BookService` + `BookClient`                  |
| [`Lec14`](Lec14)  | Animation and multimedia in WPF                                    | `AnimatedUi`, `BouncingBall`, `Player`; `dotnet run Make-Media.cs` creates the media files of `Player`                                                          |
| [`Lec15`](Lec15)  | Cross-platform applications with .NET MAUI                         | `Notes`, `WaterTracker`, `WhereAmI`, `HabitTracker`, `PhotoDiary`, `CompassLevel` (Android and Windows)                                                         |
| [`Lec16`](Lec16)  | AI models in .NET applications (Microsoft.Extensions.AI)           | `Chat`, `Library`, `Providers` (user secrets), `AiAssistant` (WPF)                                                                                               |

## Notes

- `Lec05\Make-Data.ps1` generates `search-data`, `hash` (about 420 MB) and `photos` next to the
  projects. The folder boxes of `FileSearch`, `Checksums` and `Resize` start with the author's path:
  pick your own `Lec05\…` folder in the app.
- `Lec07\Setup-Db.ps1` creates the PostgreSQL role and databases of topics 7–8 and stores the
  connection strings in the user secrets of the projects (run it once; `psql` asks for the
  `postgres` password).
- `Lec03\PhoneBook` saves its contacts to `%APPDATA%\PhoneBook\contacts.json`.
- `Lec03\TextEditor\notes.txt` keeps CRLF line endings on purpose (see `Lec03\.gitattributes`).
- Build output (`bin`, `obj`, `.vs`) and generated data are ignored by `.gitignore`.
