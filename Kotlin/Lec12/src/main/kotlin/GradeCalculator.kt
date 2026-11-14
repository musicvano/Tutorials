fun passed(score: Int): Boolean {
    require(score in 0..100) { "score outside 0..100" }
    return score >= 60
}

fun averageScore(scores: List<Int>): Double {
    require(scores.isNotEmpty()) { "empty scores" }
    require(scores.all { it in 0..100 })
    return scores.map { it.toDouble() }.average()
}
