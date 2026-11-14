import java.nio.file.Files
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
    val path = Files.createTempFile("library-", ".db")
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
        (Books innerJoin Authors)
            .select(Books.title, Authors.name)
            .orderBy(Books.id)
            .forEach {
                println("${it[Books.title]}: ${it[Authors.name]}")
            }
        val count = Books.id.count()
        (Authors innerJoin Books)
            .select(Authors.name, count)
            .groupBy(Authors.id, Authors.name)
            .orderBy(Authors.id)
            .forEach { println("${it[Authors.name]}: ${it[count]}") }
    }
    path.toFile().deleteOnExit()
}
