fun main() {
    val score = 84
    val grade = when (score) {
        in 90..100 -> "A"
        in 82..89 -> "B"
        in 74..81 -> "C"
        in 64..73 -> "D"
        in 60..63 -> "E"
        in 35..59 -> "FX"
        in 0..34 -> "F"
        else -> "помилка"
    }
    println(grade)
}
