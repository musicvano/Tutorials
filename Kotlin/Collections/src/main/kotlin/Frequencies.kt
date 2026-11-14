fun frequencies(text: String): Map<String, Int> {
    val counts = mutableMapOf<String, Int>()
    for (word in text.lowercase().split(' ')) {
        if (word.isBlank()) continue
        counts[word] = counts.getOrDefault(word, 0) + 1
    }
    return counts.toSortedMap()
}

fun main() {
    val counts = frequencies("red blue red green blue red")
    for ((word, count) in counts) {
        println("$word: $count")
    }
    val positions = mutableMapOf<String, MutableList<Int>>()
    val tokens = listOf("red", "blue", "red")
    for ((index, word) in tokens.withIndex()) {
        positions.getOrPut(word) { mutableListOf() }.add(index)
    }
    println(positions)
    println(counts["black"])
}
