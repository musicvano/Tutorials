# Object-oriented programming in Java

Code examples for the course [Object-oriented programming in Java](https://mvano.com/courses/java/).
Each `LecNN` folder belongs to topic NN of the course.

## Tools

- JDK 27 (the projects expect an IntelliJ IDEA SDK named `27`) and IntelliJ IDEA 2026.2.
- Maven projects bring their wrapper (`mvnw.cmd` → Maven 3.9.16), the Gradle project its own
  (`gradlew.bat` → Gradle 9.7.0). Set `JAVA_HOME` to JDK 27 before running a wrapper.
- Topics 14 and 16: PostgreSQL 18.

Open a `LecNN` folder (or the Maven/Gradle project inside it) in IntelliJ IDEA. From the command line:

```powershell
$env:JAVA_HOME = 'C:\Program Files\Java\jdk-27'
javac -encoding UTF-8 -d Lec04\out Lec04\src\*.java
java -cp Lec04\out PositiveAverage
cd Lec13\loan; .\mvnw.cmd package
```

## Topics

| Folder            | Topic                                                                                   | Contents                                                                                                                                     |
| ----------------- | --------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| [`Lec01`](Lec01)  | The Java platform and JDK 27, IntelliJ IDEA, program structure, Git                     | IntelliJ project `Hello`: `Hello.java`, `EnvironmentInfo.java`; run configuration `Hello` with a program argument                            |
| [`Lec02`](Lec02)  | Data types, variables, operations, console input/output, and control flow               | `ReadCount.java`, `FunctionTable.java`                                                                                                      |
| [`Lec03`](Lec03)  | Methods, arrays, and strings: `String`, `StringBuilder`, and text blocks                | `ConcatLoop.java` (string concatenation in a loop and the IDE quick fix)                                                                    |
| [`Lec04`](Lec04)  | Exception handling and debugging                                                        | `Payment.java`, `AssertionDemo.java` (run configuration with `-ea`), `PositiveAverage.java`                                                 |
| [`Lec05`](Lec05)  | Classes and objects: fields, methods, constructors, encapsulation, and packages         | Modules `Accounts`, `Library` (packages, `javac` / `jar` from the terminal), `Temperatures` (Javadoc)                                       |
| [`Lec06`](Lec06)  | Inheritance, polymorphism, and the `Object` class                                       | Modules `Payroll`, `Points` (`equals` and `hashCode`)                                                                                        |
| [`Lec07`](Lec07)  | Abstract classes, interfaces, and nested classes                                        | `Main.java` (`Comparable` / `Comparator`), `CatalogService.java` (for *Extract Interface*)                                                 |
| [`Lec08`](Lec08)  | Records, enums, sealed classes, and pattern matching                                    | `Main.java` (sealed `Shape`), `Point.java` (for *Convert to record*)                                                                        |
| [`Lec09`](Lec09)  | Generic programming                                                                     | `Main.java` (`Stack<E>`), `RawList.java` (raw types)                                                                                         |
| [`Lec10`](Lec10)  | Java Collections Framework: lists, sets, queues, and maps                               | `Main.java` (word frequencies)                                                                                                              |
| [`Lec11`](Lec11)  | Lambda expressions, functional interfaces, and the Stream API                           | `Main.java`, `AnonymousRule.java`, `StreamTrace.java`                                                                                       |
| [`Lec12`](Lec12)  | Files: I/O streams, NIO.2, and object serialization                                     | `Main.java`, `PathInfo.java`, `ReadText.java`; `data\input.txt`; run configurations with working directory `data`                          |
| [`Lec13`](Lec13)  | The Java module system, Maven and Gradle, unit testing with JUnit                       | Modules `ua.knu.converter` and `ua.knu.app`; `loan` (Maven, JUnit tests); `loan-gradle` (the same with Gradle)                              |
| [`Lec14`](Lec14)  | Relational databases with JDBC                                                          | `library` (Maven, pgJDBC, examples 1–4); `setup-library.sql` / `drop-library.sql`; `demo-schema.sql`, `join-query.sql`                     |
| [`Lec15`](Lec15)  | JavaFX: scenes, controls, layout, and events                                            | Maven project `fx-course` (JavaFX 27): `CounterMain`, `TemperatureMain`, `PaintMain`, `GreetingMain` (FXML + CSS)                         |
| [`Lec16`](Lec16)  | MVC, data binding, and database access in JavaFX applications                           | Maven project (JavaFX 27, pgJDBC): `PropertyMain`, `BindingMain`, `TableMain`, `BookMain`; layered `BookApp`; `j16-schema.sql`; `Launch.ps1` |

## Notes

- Databases (topics 14, 16): run `Lec14\setup-library.sql` as `postgres` (it creates the role `lab`
  and the database `library`; `\password` asks for the password). The run configurations of
  `Lec14\library` read `db.env`: copy `db.env.example` to `db.env` and fill in the password.
  `Lec16\Launch.ps1 BookMain` reads the connection from `COURSE_DB_URL`, `COURSE_DB_USER` and
  `COURSE_DB_PASSWORD`.
- Build output (`out`, `target`, `build`, `.gradle`), per-user IntelliJ files and `db.env` are
  ignored by `.gitignore`; project files (`.iml`, `.idea\misc.xml`, `modules.xml`, shared run
  configurations) are kept so the projects open ready to run.
