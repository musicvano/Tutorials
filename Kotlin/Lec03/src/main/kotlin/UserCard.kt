val phonePattern = Regex("\\+380[0-9]{9}")

fun userCard(name: String, phone: String, width: Int = 12): String {
    require(name.isNotBlank())
    require(phonePattern.matches(phone)) { "invalid phone format" }
    require(width in 6..30)
    return """
        |${"Name".padEnd(width)}: ${name.trim()}
        |${"Phone".padEnd(width)}: $phone
    """.trimMargin()
}

fun main() {
    println(userCard("Ada", "+380501112233", width = 6))
    val versionPattern = Regex("([0-9]+)\\.([0-9]+)")
    val match = versionPattern.find("version 2.4 ready")
    if (match != null) {
        println("major=${match.groupValues[1]}")
        println("minor=${match.groupValues[2]}")
    }
    println(phonePattern.matches("x+380501112233"))
}
