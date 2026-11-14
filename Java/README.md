# Object-oriented programming in Java

Code examples for the course [Object-oriented programming in Java](https://mvano.com/courses/java/).
Each topic of the course has its own folder named after the topic; the table below lists them in
course order and links each topic page.

## Tools

- JDK 27 (the projects expect an IntelliJ IDEA SDK named `27`) and IntelliJ IDEA 2026.2.
- Maven projects bring their wrapper (`mvnw.cmd` → Maven 3.9.16), the Gradle project its own
  (`gradlew.bat` → Gradle 9.7.0). Set `JAVA_HOME` to JDK 27 before running a wrapper.
- Topics 14 and 16: PostgreSQL 18.

Open a topic folder (or the Maven/Gradle project inside it) in IntelliJ IDEA. From the command line:

```powershell
$env:JAVA_HOME = 'C:\Program Files\Java\jdk-27'
javac -encoding UTF-8 -d Exceptions\out Exceptions\src\*.java
java -cp Exceptions\out PositiveAverage
cd Modules\loan; .\mvnw.cmd package
```

## Topics

| #                                                                | Folder                       | Topic                                                                           | Contents                                                                                                                                     |
| ---------------------------------------------------------------- | ---------------------------- | ------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| [1](https://mvano.com/courses/java/01-intro/)                    | [`Intro`](Intro)             | The Java platform and JDK 27, IntelliJ IDEA, program structure, Git             | IntelliJ project `Hello`: `Hello.java`, `EnvironmentInfo.java`; run configuration `Hello` with a program argument                            |
| [2](https://mvano.com/courses/java/02-types-control-flow/)       | [`ControlFlow`](ControlFlow) | Data types, variables, operations, console input/output, and control flow       | `ReadCount.java`, `FunctionTable.java`                                                                                                       |
| [3](https://mvano.com/courses/java/03-methods-arrays-strings/)   | [`Methods`](Methods)         | Methods, arrays, and strings: `String`, `StringBuilder`, and text blocks        | `ConcatLoop.java` (string concatenation in a loop and the IDE quick fix)                                                                     |
| [4](https://mvano.com/courses/java/04-exceptions-debugging/)     | [`Exceptions`](Exceptions)   | Exception handling and debugging                                                | `Payment.java`, `AssertionDemo.java` (run configuration with `-ea`), `PositiveAverage.java`                                                  |
| [5](https://mvano.com/courses/java/05-classes/)                  | [`Classes`](Classes)         | Classes and objects: fields, methods, constructors, encapsulation, and packages | Modules `Accounts`, `Library` (packages, `javac` / `jar` from the terminal), `Temperatures` (Javadoc)                                        |
| [6](https://mvano.com/courses/java/06-inheritance-polymorphism/) | [`Inheritance`](Inheritance) | Inheritance, polymorphism, and the `Object` class                               | Modules `Payroll`, `Points` (`equals` and `hashCode`)                                                                                        |
| [7](https://mvano.com/courses/java/07-abstract-interfaces/)      | [`Interfaces`](Interfaces)   | Abstract classes, interfaces, and nested classes                                | `Main.java` (`Comparable` / `Comparator`), `CatalogService.java` (for *Extract Interface*)                                                   |
| [8](https://mvano.com/courses/java/08-records-enums-sealed/)     | [`Records`](Records)         | Records, enums, sealed classes, and pattern matching                            | `Main.java` (sealed `Shape`), `Point.java` (for *Convert to record*)                                                                         |
| [9](https://mvano.com/courses/java/09-generics/)                 | [`Generics`](Generics)       | Generic programming                                                             | `Main.java` (`Stack<E>`), `RawList.java` (raw types)                                                                                         |
| [10](https://mvano.com/courses/java/10-collections/)             | [`Collections`](Collections) | Java Collections Framework: lists, sets, queues, and maps                       | `Main.java` (word frequencies)                                                                                                               |
| [11](https://mvano.com/courses/java/11-lambdas-streams/)         | [`Streams`](Streams)         | Lambda expressions, functional interfaces, and the Stream API                   | `Main.java`, `AnonymousRule.java`, `StreamTrace.java`                                                                                        |
| [12](https://mvano.com/courses/java/12-io-files/)                | [`Files`](Files)             | Files: I/O streams, NIO.2, and object serialization                             | `Main.java`, `PathInfo.java`, `ReadText.java`; `data\input.txt`; run configurations with working directory `data`                            |
| [13](https://mvano.com/courses/java/13-modules-build-testing/)   | [`Modules`](Modules)         | The Java module system, Maven and Gradle, unit testing with JUnit               | Modules `ua.knu.converter` and `ua.knu.app`; `loan` (Maven, JUnit tests); `loan-gradle` (the same with Gradle)                               |
| [14](https://mvano.com/courses/java/14-jdbc/)                    | [`Jdbc`](Jdbc)               | Relational databases with JDBC                                                  | `library` (Maven, pgJDBC, examples 1–4); `setup-library.sql` / `drop-library.sql`; `demo-schema.sql`, `join-query.sql`                       |
| [15](https://mvano.com/courses/java/15-javafx/)                  | [`JavaFx`](JavaFx)           | JavaFX: scenes, controls, layout, and events                                    | Maven project `fx-course` (JavaFX 27): `CounterMain`, `TemperatureMain`, `PaintMain`, `GreetingMain` (FXML + CSS)                            |
| [16](https://mvano.com/courses/java/16-mvc-data-binding/)        | [`Mvc`](Mvc)                 | MVC, data binding, and database access in JavaFX applications                   | Maven project (JavaFX 27, pgJDBC): `PropertyMain`, `BindingMain`, `TableMain`, `BookMain`; layered `BookApp`; `j16-schema.sql`; `Launch.ps1` |

## Notes

- Databases (topics 14, 16): run `Jdbc\setup-library.sql` as `postgres` (it creates the role `lab`
  and the database `library`; `\password` asks for the password). The run configurations of
  `Jdbc\library` read `db.env`: copy `db.env.example` to `db.env` and fill in the password.
  `Mvc\Launch.ps1 BookMain` reads the connection from `COURSE_DB_URL`, `COURSE_DB_USER` and
  `COURSE_DB_PASSWORD`.
- Build output (`out`, `target`, `build`, `.gradle`), per-user IntelliJ files and `db.env` are
  ignored by `.gitignore`; project files (`.iml`, `.idea\misc.xml`, `modules.xml`, shared run
  configurations) are kept so the projects open ready to run.
