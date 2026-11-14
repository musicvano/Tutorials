import java.nio.file.Files
import org.jetbrains.exposed.v1.core.Table
import org.jetbrains.exposed.v1.jdbc.*
import org.jetbrains.exposed.v1.jdbc.transactions.transaction

object Shelves : Table("shelves") {
    val id = integer("id").autoIncrement()
    val name = varchar("name", 80).uniqueIndex()
    override val primaryKey = PrimaryKey(id)
}

fun main() {
    val path = Files.createTempFile("shelves-", ".db")
    val db = Database.connect(
        "jdbc:sqlite:$path?foreign_keys=on", "org.sqlite.JDBC"
    )
    transaction(db) {
        maxAttempts = 1
        SchemaUtils.create(Shelves)
        Shelves.insert { it[name] = "Поезія" }
        println("Полиць: ${Shelves.selectAll().count()}")
    }
    path.toFile().deleteOnExit()
}
