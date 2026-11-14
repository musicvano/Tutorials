@JvmInline
value class UserId(val value: Long) {
    init { require(value > 0) }
}

class User private constructor(
    val id: UserId,
    val name: String
) {
    companion object {
        const val MAX_NAME = 40
        fun create(id: UserId, rawName: String): User {
            val name = rawName.trim()
            require(name.isNotEmpty() && name.length <= MAX_NAME)
            return User(id, name)
        }
    }

    override fun toString(): String = "${id.value}: $name"
}

object AppConfig {
    const val TITLE = "Study users"
}

fun main() {
    println(AppConfig.TITLE)
    val user = User.create(UserId(7), "  Olena  ")
    println(user)
    try {
        User.create(UserId(8), "   ")
    } catch (e: IllegalArgumentException) {
        println("Invalid name")
    }
}
