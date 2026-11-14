data class Book(val title: String, val year: Int) {
    var viewed: Boolean = false
    init {
        require(title.isNotBlank())
        require(year in 1450..2100)
    }
}

fun main() {
    val first = Book("Kotlin notes", 2025)
    first.viewed = true
    val same = Book("Kotlin notes", 2025)
    val revised = first.copy(year = 2026)
    val (title, year) = revised
    println(first)
    println("equal=${first == same}")
    println("identity=${first === same}")
    println("$title / $year")
    println("viewed=${revised.viewed}")
    println("hash equal=${first.hashCode() == same.hashCode()}")
}
