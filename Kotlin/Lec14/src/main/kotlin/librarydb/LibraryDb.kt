// Screenshot 14/06 (not a lecture listing): the schema and data of example 2
// (Catalog.kt) written to a kept file for DataGrip instead of a temp file.
// Run: .\gradlew.bat :Lec14:libraryDb  ->  Code\Lec14\library.db (git-ignored).
package librarydb

import java.nio.file.Files
import java.nio.file.Path
import org.jetbrains.exposed.v1.core.*
import org.jetbrains.exposed.v1.jdbc.*
import org.jetbrains.exposed.v1.jdbc.transactions.transaction

object Authors : Table("authors") {
    val id = integer("id").autoIncrement()
    val name = varchar("name", 100)
    override val primaryKey = PrimaryKey(id)
}
object Books : Table("books") {
    val id = integer("id").autoIncrement()
    val title = varchar("title", 120)
    val author = integer("author_id").references(Authors.id)
    override val primaryKey = PrimaryKey(id)
}

fun main() {
    val path = Path.of(System.getProperty("library.db", "library.db"))
        .toAbsolutePath()
    Files.deleteIfExists(path)
    val db = Database.connect(
        "jdbc:sqlite:$path?foreign_keys=on", "org.sqlite.JDBC"
    )
    transaction(db) {
        SchemaUtils.create(Authors, Books)
        val a = Authors.insert { it[name] = "Автор A" }[Authors.id]
        val b = Authors.insert { it[name] = "Автор B" }[Authors.id]
        for ((title, author) in listOf(
            "Весна" to a, "Літо" to a, "Осінь" to b
        )) {
            Books.insert {
                it[Books.title] = title
                it[Books.author] = author
            }
        }
        println("Книг: ${Books.selectAll().count()}")
    }
    println("Файл: $path")
}
