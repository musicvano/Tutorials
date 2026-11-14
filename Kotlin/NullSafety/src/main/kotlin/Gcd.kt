fun main() {
    print("Перше число 1–1000000: ")
    val first = readlnOrNull()?.toIntOrNull()
    print("Друге число 1–1000000: ")
    val second = readlnOrNull()?.toIntOrNull()
    if (first == null || second == null ||
        first !in 1..1000000 || second !in 1..1000000
    ) {
        println("Потрібні два додатні цілі числа")
        return
    }
    var a: Int = first
    var b: Int = second
    var steps = 0
    while (b != 0) {
        val remainder = a % b
        a = b
        b = remainder
        steps++
    }
    println("НСД: $a")
    println("Кроків: $steps")
}
