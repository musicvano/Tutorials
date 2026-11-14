# Object-oriented programming in Kotlin

Code examples for the course [Object-oriented programming in Kotlin](https://mvano.com/courses/kotlin/).
This folder is one Gradle build (Kotlin DSL) with one subproject per topic, named after
the topic; the table below lists them in course order and links each topic page.
Each listing is a file with its own `main`, so several examples live side by side in one subproject.

## Tools

- OpenJDK 27 for the programs (`jvmToolchain(27)`) and JDK 25 to run Gradle 9.7.0 (the wrapper
  downloads Gradle itself).
- IntelliJ IDEA 2026.2: open this `Kotlin` folder as the project, set *Settings → Build, Execution,
  Deployment → Build Tools → Gradle → Gradle JVM* to JDK 25 and the project SDK to JDK 27.
- Versions: Kotlin 2.4.20, kotlinx.serialization, kotlinx.coroutines, Exposed + SQLite,
  Compose Multiplatform 1.11.1 (declared in the subproject build files and the root `build.gradle.kts`).

```powershell
$env:JAVA_HOME = 'C:\Program Files\Java\jdk-25.0.4.1'
.\gradlew.bat build                  # all subprojects and their tests
.\gradlew.bat :NullSafety:run             # run a subproject's main program
.\gradlew.bat :Exposed:libraryDb       # creates Exposed\library.db
```

If Gradle does not find JDK 27, add
`-Porg.gradle.java.installations.paths=C:/Program Files/Java/jdk-27` to the command (or put it into
`%USERPROFILE%\.gradle\gradle.properties`).

## Topics

| #                                                                      | Folder                                 | Topic                                                               | Contents                                                                                                |
| ---------------------------------------------------------------------- | -------------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------- |
| [1](https://mvano.com/courses/kotlin/01-intro/)                        | [`Intro`](Intro)                       | Kotlin 2.4 and the JVM, IntelliJ IDEA, Gradle projects, Git         | `Main.kt`, `BirthYear.kt`, `Travel.kt`, `Args.kt`; `hello\` is the standalone project of the lecture    |
| [2](https://mvano.com/courses/kotlin/02-types-null-safety/)            | [`NullSafety`](NullSafety)             | Data types, variables, null safety, operations, and control flow    | `Gcd.kt`, `Overflow.kt`, `Grade.kt`                                                                     |
| [3](https://mvano.com/courses/kotlin/03-functions-strings/)            | [`Functions`](Functions)               | Functions, named arguments, extension functions, strings            | `Price.kt`, `PriceHints.kt`, `StringExt.kt`, `UserCard.kt`                                              |
| [4](https://mvano.com/courses/kotlin/04-exceptions-debugging/)         | [`Exceptions`](Exceptions)             | Exception handling, the `Result` type, and debugging                | `Division.kt`, `Port.kt`, `PortCrash.kt`, `ResultMapping.kt`, `Average.kt`                              |
| [5](https://mvano.com/courses/kotlin/05-classes/)                      | [`Classes`](Classes)                   | Classes and objects: properties, constructors, visibility, packages | `Account.kt`; packages `ua.edu.bank` and `ua.edu.app`                                                   |
| [6](https://mvano.com/courses/kotlin/06-inheritance-interfaces/)       | [`Inheritance`](Inheritance)           | Inheritance, abstract classes, interfaces, and polymorphism         | `Shapes.kt`                                                                                             |
| [7](https://mvano.com/courses/kotlin/07-data-enum-sealed/)             | [`DataClasses`](DataClasses)           | Data classes, enums, sealed classes, objects, and companion objects | `Book.kt`, `Order.kt`, `Users.kt`                                                                       |
| [8](https://mvano.com/courses/kotlin/08-operators-delegation/)         | [`Delegation`](Delegation)             | Operator overloading, delegated properties, and class delegation    | `Vector.kt`, `Settings.kt`                                                                              |
| [9](https://mvano.com/courses/kotlin/09-generics/)                     | [`Generics`](Generics)                 | Generic programming: variance and reified parameters                | `MutableBox.kt`, `Parse.kt`                                                                             |
| [10](https://mvano.com/courses/kotlin/10-collections/)                 | [`Collections`](Collections)           | Collections: lists, sets, maps                                      | `Frequencies.kt`                                                                                        |
| [11](https://mvano.com/courses/kotlin/11-lambdas-sequences/)           | [`Lambdas`](Lambdas)                   | Lambdas, higher-order functions, collection operations, sequences   | `OrderReport.kt`, `Primes.kt`                                                                           |
| [12](https://mvano.com/courses/kotlin/12-files-serialization-testing/) | [`Serialization`](Serialization)       | Files, kotlinx.serialization, and unit testing                      | `Catalog.kt`, `GradeCalculator.kt` + tests; an included build with the lecture's own `build.gradle.kts` |
| [13](https://mvano.com/courses/kotlin/13-coroutines-flow/)             | [`Coroutines`](Coroutines)             | Coroutines: structured concurrency, dispatchers, Flow               | `Breakfast.kt`, `Download.kt`, `Deposit.kt`, `Sensor.kt`, `debugnames\NamedBreakfast.kt`; tests         |
| [14](https://mvano.com/courses/kotlin/14-exposed-databases/)           | [`Exposed`](Exposed)                   | Relational databases with Exposed                                   | `Shelves.kt`, `Catalog.kt`, `sqllog\ShelvesLog.kt`, `librarydb\LibraryDb.kt`; PostgreSQL setup scripts  |
| [15](https://mvano.com/courses/kotlin/15-compose/)                     | [`Compose`](Compose)                   | Compose Multiplatform: components, state, and layout                | `counter\Counter.kt`, `Main.kt` (temperature converter), `Tasks.kt`, `TemperaturePreview.kt`            |
| [15](https://mvano.com/courses/kotlin/15-compose/)                     | [`ComposeHotReload`](ComposeHotReload) | Compose Hot Reload                                                  | The counter of topic 15 on JDK 25 (Hot Reload needs a JetBrains Runtime of the toolchain version)       |
| [16](https://mvano.com/courses/kotlin/16-mvvm-navigation/)             | [`Mvvm`](Mvvm)                         | MVVM, navigation, and database access in Compose Multiplatform      | `Main.kt` (notes app), `Counter.kt`, `Navigation.kt`; tests; MSI packaging                              |

## Notes

- Console input in IntelliJ (topics 2, 4): if the Run console does not accept input, set *Build and
  run using: IntelliJ IDEA* in the Gradle settings, or run `.\gradlew.bat :NullSafety:run`.
- `:Compose:run` starts Compose on the JDK Gradle runs on (25), but the bytecode targets 26: start
  topic 15 from the gutter icon in the editor, or run Gradle on JDK 27.
- `Mvvm` stores its notes in `%USERPROFILE%\.course-notes\notes.db`;
  `.\gradlew.bat :Mvvm:packageDistributionForCurrentOS` builds the MSI installer.
- Gradle caches (`.gradle`, `.kotlin`), `build`, IDE settings and `*.db` are ignored by `.gitignore`.
