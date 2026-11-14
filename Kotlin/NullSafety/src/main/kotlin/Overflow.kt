fun main() {
    val count = 50000
    val unsafe = count * count
    val safe = count.toLong() * count
    println(unsafe)
    println(safe)
    println(3.9.toInt())
    println((-3.9).toInt())
}
