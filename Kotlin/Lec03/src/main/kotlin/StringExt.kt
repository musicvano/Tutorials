fun String.isPalindrome(): Boolean {
    val cleaned = StringBuilder()
    for (character in this) {
        if (!character.isWhitespace()) {
            cleaned.append(character.lowercaseChar())
        }
    }
    var left = 0
    var right = cleaned.length - 1
    while (left < right) {
        if (cleaned[left] != cleaned[right]) return false
        left++
        right--
    }
    return true
}

val String.wordCount: Int
    get() {
        var count = 0
        var inside = false
        for (character in this) {
            if (character.isWhitespace()) {
                inside = false
            } else if (!inside) {
                count++
                inside = true
            }
        }
        return count
    }

fun String?.orLabel(): String = this ?: "missing"

fun main() {
    println("Never odd or even".isPalindrome())
    println("Kotlin".isPalindrome())
    println("  Ada\tLovelace  ".wordCount)
    println("".isPalindrome())
    println((null as String?).orLabel())
}
