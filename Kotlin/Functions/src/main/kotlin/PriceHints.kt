fun main() {
    val positional = formatPrice(1250.5, "UAH", 2)
    val named = formatPrice(amount = 1250.5, currency = "UAH", digits = 2)
    println(positional)
    println(named)
}
