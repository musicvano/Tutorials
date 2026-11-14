fun main() {
    val source = Result.success("12")
    val parsed = source.mapCatching { it.toInt() }
    println(parsed.getOrElse { -1 })
    val invalid = Result.success("abc")
        .mapCatching { it.toInt() }
    println(invalid.fold(
        onSuccess = { "Число: $it" },
        onFailure = { "Неправильне число" }
    ))
}
