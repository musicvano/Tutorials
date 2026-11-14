fun positiveAverage(values: IntArray): Double? {
    var sum = 0L
    var count = 0
    for (value in values) {
        if (value > 0) {
            sum += value
            count++
        }
    }
    return if (count == 0) null else sum.toDouble() / count
}

fun main() {
    val values = intArrayOf(10, -5, 20)
    println(positiveAverage(values))
    println(positiveAverage(intArrayOf(-2, 0)))
    check(positiveAverage(values) == 15.0)
    check(positiveAverage(intArrayOf()) == null)
}
