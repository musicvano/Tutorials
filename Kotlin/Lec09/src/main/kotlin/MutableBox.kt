class MutableBox<T>(var value: T)

fun main() {
    val words = MutableBox("hello")
    // Навмисна помилка: зніміть коментар окремо.
    // val anything: MutableBox<Any> = words
    println(words.value.length)
}
