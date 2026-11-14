# Tutorials

Code examples for the university courses by Ivan Muzyka. The lectures, labs and slides are on [mvano.com](https://mvano.com/); this repository holds the programs they discuss, so you can open, run, debug and change them yourself.

| Folder                   | Course                                  | Course page                                                           |
| ------------------------ | --------------------------------------- | --------------------------------------------------------------------- |
| [`Cpp`](Cpp)             | Object-oriented programming in C++      | [mvano.com/courses/cpp](https://mvano.com/courses/cpp/)               |
| [`CSharp`](CSharp)       | Object-oriented programming in C#       | [mvano.com/courses/csharp](https://mvano.com/courses/csharp/)         |
| [`CSharpGui`](CSharpGui) | Object-oriented programming in C# (GUI) | [mvano.com/courses/csharp-gui](https://mvano.com/courses/csharp-gui/) |
| [`Java`](Java)           | Object-oriented programming in Java     | [mvano.com/courses/java](https://mvano.com/courses/java/)             |
| [`Kotlin`](Kotlin)       | Object-oriented programming in Kotlin   | [mvano.com/courses/kotlin](https://mvano.com/courses/kotlin/)         |
| [`Parallel`](Parallel)   | Parallel and distributed computing      | [mvano.com/courses/parallel](https://mvano.com/courses/parallel/)     |
| [`Python`](Python)       | Object-oriented programming in Python   | [mvano.com/courses/python](https://mvano.com/courses/python/)         |

Inside a course folder, each topic of the course has its own folder named after the topic (`CSharp/Intro`, `Cpp/Functions`); the course `README.md` lists them in course order and links each topic page. A topic without examples has no folder. Each course folder has its own `README.md` with the tools it needs and how to build and run the programs.

## Getting the code

Clone the whole repository:

```
git clone https://github.com/musicvano/Tutorials.git
```

Or take only the course you study (Git 2.25 or later):

```
git clone --filter=blob:none --sparse https://github.com/musicvano/Tutorials.git
cd Tutorials
git sparse-checkout set CSharp
```

Without Git, download the [ZIP archive](https://github.com/musicvano/Tutorials/archive/refs/heads/main.zip) of the whole repository.

To keep your own changes apart from the originals, copy a topic folder (or the project you need) somewhere else, or work on a branch of your clone: `git pull` then brings in updates without conflicts.

## Tools

| Folder      | You need                                                                                                          |
| ----------- | ----------------------------------------------------------------------------------------------------------------- |
| `Cpp`       | Visual Studio 2026 with the _Desktop development with C++_ workload                                               |
| `CSharp`    | .NET 10 SDK; Visual Studio 2026, JetBrains Rider or VS Code with C# Dev Kit                                       |
| `CSharpGui` | .NET 10 SDK and Visual Studio 2026 on Windows (Windows Forms, WPF, .NET MAUI); PostgreSQL, Ollama for some topics |
| `Java`      | JDK 27 and IntelliJ IDEA; PostgreSQL for topics 14 and 16                                                         |
| `Kotlin`    | JDK 27 (programs) and JDK 25 (Gradle), IntelliJ IDEA                                                              |
| `Parallel`  | .NET 10 SDK and JetBrains Rider; a C++ compiler with CMake, CUDA, MPI and Docker for some topics                  |
| `Python`    | Python 3.14 and PyCharm; packages per topic (see the course README)                                               |

The tool versions match the ones the lectures use; newer versions usually work too.

## License

Licensed under the [MIT License](LICENSE.md).
