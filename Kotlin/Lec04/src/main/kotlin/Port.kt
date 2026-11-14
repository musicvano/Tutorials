fun parsePort(text: String): Int {
    val port = try {
        text.toInt()
    } catch (error: NumberFormatException) {
        throw IllegalArgumentException("Порт не є числом", error)
    }
    require(port in 1..65535) { "Порт має бути1–65535" }
    return port
}

fun main() {
    println(parsePort("8080"))
    try {
        parsePort("abc")
    } catch (error: IllegalArgumentException) {
        println(error.message)
        println(error.cause?.javaClass?.simpleName)
    }
}
