# Object-oriented programming in Kotlin

Code examples for the course [Object-oriented programming in Kotlin](https://mvano.com/courses/kotlin/).
This folder is one Gradle build (Kotlin DSL) with one subproject per topic: `LecNN` belongs to topic NN.
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
.\gradlew.bat :Lec02:run             # run a subproject's main program
.\gradlew.bat :Lec14:libraryDb       # creates Lec14\library.db
```

If Gradle does not find JDK 27, add
`-Porg.gradle.java.installations.paths=C:/Program Files/Java/jdk-27` to the command (or put it into
`%USERPROFILE%\.gradle\gradle.properties`).

## Topics

| Folder                       | Topic                                                                        | Contents                                                                                                   |
| ---------------------------- | ---------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------- |
| [`Lec01`](Lec01)             | Kotlin 2.4 and the JVM, IntelliJ IDEA, Gradle projects, Git                  | `Main.kt`, `BirthYear.kt`, `Travel.kt`, `Args.kt`; `hello\` is the standalone project of the lecture      |
| [`Lec02`](Lec02)             | Data types, variables, null safety, operations, and control flow             | `Gcd.kt`, `Overflow.kt`, `Grade.kt`                                                                        |
| [`Lec03`](Lec03)             | Functions, named arguments, extension functions, strings                     | `Price.kt`, `PriceHints.kt`, `StringExt.kt`, `UserCard.kt`                                                 |
| [`Lec04`](Lec04)             | Exception handling, the `Result` type, and debugging                         | `Division.kt`, `Port.kt`, `PortCrash.kt`, `ResultMapping.kt`, `Average.kt`                                 |
| [`Lec05`](Lec05)             | Classes and objects: properties, constructors, visibility, packages          | `Account.kt`; packages `ua.edu.bank` and `ua.edu.app`                                                      |
| [`Lec06`](Lec06)             | Inheritance, abstract classes, interfaces, and polymorphism                  | `Shapes.kt`                                                                                                |
| [`Lec07`](Lec07)             | Data classes, enums, sealed classes, objects, and companion objects          | `Book.kt`, `Order.kt`, `Users.kt`                                                                          |
| [`Lec08`](Lec08)             | Operator overloading, delegated properties, and class delegation             | `Vector.kt`, `Settings.kt`                                                                                 |
| [`Lec09`](Lec09)             | Generic programming: variance and reified parameters                         | `MutableBox.kt`, `Parse.kt`                                                                                |
| [`Lec10`](Lec10)             | Collections: lists, sets, maps                                               | `Frequencies.kt`                                                                                           |
| [`Lec11`](Lec11)             | Lambdas, higher-order functions, collection operations, sequences            | `OrderReport.kt`, `Primes.kt`                                                                              |
| [`Lec12`](Lec12)             | Files, kotlinx.serialization, and unit testing                               | `Catalog.kt`, `GradeCalculator.kt` + tests; an included build with the lecture's own `build.gradle.kts`  |
| [`Lec13`](Lec13)             | Coroutines: structured concurrency, dispatchers, Flow                        | `Breakfast.kt`, `Download.kt`, `Deposit.kt`, `Sensor.kt`, `debugnames\NamedBreakfast.kt`; tests           |
| [`Lec14`](Lec14)             | Relational databases with Exposed                                            | `Shelves.kt`, `Catalog.kt`, `sqllog\ShelvesLog.kt`, `librarydb\LibraryDb.kt`; PostgreSQL setup scripts    |
| [`Lec15`](Lec15)             | Compose Multiplatform: components, state, and layout                         | `counter\Counter.kt`, `Main.kt` (temperature converter), `Tasks.kt`, `TemperaturePreview.kt`              |
| [`Lec15Reload`](Lec15Reload) | Compose Hot Reload                                                           | The counter of topic 15 on JDK 25 (Hot Reload needs a JetBrains Runtime of the toolchain version)          |
| [`Lec16`](Lec16)             | MVVM, navigation, and database access in Compose Multiplatform              | `Main.kt` (notes app), `Counter.kt`, `Navigation.kt`; tests; MSI packaging                                |

## Notes

- Console input in IntelliJ (topics 2, 4): if the Run console does not accept input, set *Build and
  run using: IntelliJ IDEA* in the Gradle settings, or run `.\gradlew.bat :Lec02:run`.
- `:Lec15:run` starts Compose on the JDK Gradle runs on (25), but the bytecode targets 26: start
  topic 15 from the gutter icon in the editor, or run Gradle on JDK 27.
- `Lec16` stores its notes in `%USERPROFILE%\.course-notes\notes.db`;
  `.\gradlew.bat :Lec16:packageDistributionForCurrentOS` builds the MSI installer.
- Gradle caches (`.gradle`, `.kotlin`), `build`, IDE settings and `*.db` are ignored by `.gitignore`.
