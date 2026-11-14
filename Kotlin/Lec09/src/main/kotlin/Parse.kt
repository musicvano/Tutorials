inline fun <reified T : Any> parse(text: String): T {
    val value: Any = when (T::class) {
        Int::class -> text.toInt()
        Double::class -> text.toDouble().also {
            require(it.isFinite()) { "finite number required" }
        }
        Boolean::class -> text.toBooleanStrict()
        else -> error("unsupported type: ${T::class.simpleName}")
    }
    return value as T
}

fun main() {
    println(parse<Int>("42") + 1)
    println(parse<Double>("2.5") * 2)
    println(parse<Boolean>("true"))
    try {
        parse<Boolean>("yes")
    } catch (error: IllegalArgumentException) {
        println("invalid boolean")
    }
    try {
        parse<String>("word")
    } catch (error: IllegalStateException) {
        println("unsupported")
    }
}
