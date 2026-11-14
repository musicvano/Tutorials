fun isPrime(number: Int): Boolean {
    if (number < 2) return false
    var divisor = 2
    while (divisor <= number / divisor) {
        if (number % divisor == 0) return false
        divisor++
    }
    return true
}

fun primes(limit: Int): Sequence<Int> = sequence {
    require(limit in 0..1_000_000)
    for (number in 2..limit) {
        if (isPrime(number)) yield(number)
    }
}

fun main() {
    println(primes(100).take(5).toList())
    println(primes(1).toList())
    var eagerCalls = 0
    val eager = (1..10).map { eagerCalls++; it * it }
        .first { it >= 9 }
    var lazyCalls = 0
    val lazy = (1..10).asSequence()
        .map { lazyCalls++; it * it }
        .first { it >= 9 }
    println("$eager $eagerCalls")
    println("$lazy $lazyCalls")
}
