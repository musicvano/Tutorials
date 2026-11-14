# Object-oriented programming in C# (GUI)

Code examples for the course [Object-oriented programming in C# (GUI)](https://mvano.com/courses/csharp-gui/):
desktop, web, network, database and AI applications in .NET. Each topic has its own folder named
after the topic, listed in course order in the table below. Topic 1 (Git) has no code folder: its lecture builds the demo repositories step by
step.

## Tools

- .NET 10 SDK and Visual Studio 2026 on Windows (Windows Forms and WPF projects target
  `net10.0-windows`).
- Topic 15: the .NET MAUI workload (`dotnet workload install maui`) and an Android emulator or Windows.
- Topics 7–8: PostgreSQL 18.
- Topic 16: [Ollama](https://ollama.com/) with the models `qwen3:4b-instruct` and `embeddinggemma`.

Open `<Topic>\<Topic>.slnx`, or build and run from the command line:

```powershell
dotnet build Sockets\Sockets.slnx
dotnet run --project Sockets\EchoServer
```

## Topics

| #                                                                  | Folder                                       | Topic                                                           | Projects                                                                                                                                               |
| ------------------------------------------------------------------ | -------------------------------------------- | --------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| [2](https://mvano.com/courses/csharp-gui/02-regex/)                | [`Regex`](Regex)                             | Regular expressions and text processing                         | `Regexes` (date pattern, `dates.txt`), `Censor` (`[GeneratedRegex]`), `Contacts.Core` + `Contacts.Tests` (xUnit v3, one test fails on purpose)         |
| [3](https://mvano.com/courses/csharp-gui/03-winforms/)             | [`WinForms`](WinForms)                       | Windows Forms fundamentals                                      | `TemperatureConverter` (`ErrorProvider`), `LayoutDemo` (`TableLayoutPanel`), `TextEditor`, `BookCatalog`, `BmiCalculator`, `StopwatchApp`, `PhoneBook` |
| [4](https://mvano.com/courses/csharp-gui/04-gdi-plus/)             | [`GdiPlus`](GdiPlus)                         | Graphics with Windows Forms and GDI+                            | `Graph` (y = sin(x)·x), `Clock` (analog clock), `Editor` (paint editor, Save PNG, Print Preview), `Gauge` (custom control)                             |
| [5](https://mvano.com/courses/csharp-gui/05-async-await/)          | [`Async`](Async)                             | Asynchronous programming with async/await in GUI applications   | `Primes`, `FileSearch`, `Pages`, `Checksums`, `Mandelbrot`, `Resize`; `Make-Data.ps1` creates their test data                                          |
| [6](https://mvano.com/courses/csharp-gui/06-dependency-injection/) | [`DependencyInjection`](DependencyInjection) | Dependency injection, configuration, and logging                | `Notifications`, `ConfigDemo` (launch profile, user secrets), `WeatherMonitor` (Generic Host worker), `TodoApp` (Windows Forms + host)                 |
| [7](https://mvano.com/courses/csharp-gui/07-sql-ado-net/)          | [`AdoNet`](AdoNet)                           | ADO.NET: relational databases and SQL                           | `Library`, `Injection` (SQL injection and parameters), `BookBrowser`; SQL scripts; `Setup-Db.ps1` creates the databases                                |
| [8](https://mvano.com/courses/csharp-gui/08-ef-core/)              | [`EfCore`](EfCore)                           | Object-relational mapping with Entity Framework Core            | `Blog`, `Shop`, `States`, `Stock`                                                                                                                      |
| [9](https://mvano.com/courses/csharp-gui/09-sockets/)              | [`Sockets`](Sockets)                         | Network applications with `Socket`                              | `EchoServer`, `EchoClient`, `ChatServer`, `ChatClient` (Windows Forms)                                                                                 |
| [10](https://mvano.com/courses/csharp-gui/10-rest-api/)            | [`RestApi`](RestApi)                         | REST web services: ASP.NET Core Minimal API, `HttpClient`, JSON | `TodoApi` (ports 5080/7080, `TodoApi.http`), `TodoClient`                                                                                              |
| [11](https://mvano.com/courses/csharp-gui/11-signalr/)             | [`SignalR`](SignalR)                         | Real-time messaging with ASP.NET Core SignalR                   | `ChatServer`, `ChatClient`, `RoomsClient`, `BoardServer`, `BoardClient`                                                                                |
| [12](https://mvano.com/courses/csharp-gui/12-wpf/)                 | [`Wpf`](Wpf)                                 | Windows Presentation Foundation: XAML, layout, routed events    | `Registration`, `RegistrationFluent` (argument `dark`), `UnitConverter`, `EventDemo`, `FileManager`                                                    |
| [13](https://mvano.com/courses/csharp-gui/13-wpf-mvvm/)            | [`Mvvm`](Mvvm)                               | Data binding, styles, templates, and MVVM in WPF                | `Profile`, `Catalog`, `Styles`, `Registration`, `TodoApp` + `TodoApp.Core` + `TodoApp.Tests`, `Schedule`, `Cards`, `BookService` + `BookClient`        |
| [14](https://mvano.com/courses/csharp-gui/14-wpf-animation/)       | [`Animation`](Animation)                     | Animation and multimedia in WPF                                 | `AnimatedUi`, `BouncingBall`, `Player`; `dotnet run Make-Media.cs` creates the media files of `Player`                                                 |
| [15](https://mvano.com/courses/csharp-gui/15-maui/)                | [`Maui`](Maui)                               | Cross-platform applications with .NET MAUI                      | `Notes`, `WaterTracker`, `WhereAmI`, `HabitTracker`, `PhotoDiary`, `CompassLevel` (Android and Windows)                                                |
| [16](https://mvano.com/courses/csharp-gui/16-ai/)                  | [`Ai`](Ai)                                   | AI models in .NET applications (Microsoft.Extensions.AI)        | `Chat`, `Library`, `Providers` (user secrets), `AiAssistant` (WPF)                                                                                     |

## Notes

- `Async\Make-Data.ps1` generates `search-data`, `hash` (about 420 MB) and `photos` next to the
  projects. The folder boxes of `FileSearch`, `Checksums` and `Resize` start with the author's path:
  pick your own `Async\…` folder in the app.
- `AdoNet\Setup-Db.ps1` creates the PostgreSQL role and databases of topics 7–8 and stores the
  connection strings in the user secrets of the projects (run it once; `psql` asks for the
  `postgres` password).
- `WinForms\PhoneBook` saves its contacts to `%APPDATA%\PhoneBook\contacts.json`.
- `WinForms\TextEditor\notes.txt` keeps CRLF line endings on purpose (see `WinForms\.gitattributes`).
- Build output (`bin`, `obj`, `.vs`) and generated data are ignored by `.gitignore`.
