// Screenshot 14/05: example 1 of the lecture (Shelves.kt) with the SQL logger
// the lecture adds right after it; own package so Shelves does not clash.
package sqllog

import java.nio.file.Files
import org.jetbrains.exposed.v1.core.StdOutSqlLogger
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
        addLogger(StdOutSqlLogger)
        SchemaUtils.create(Shelves)
        Shelves.insert { it[name] = "Поезія" }
        println("Полиць: ${Shelves.selectAll().count()}")
    }
    path.toFile().deleteOnExit()
}
