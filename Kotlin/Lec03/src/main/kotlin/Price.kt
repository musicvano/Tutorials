import java.util.Locale

fun discountedPrice(amount: Double, discount: Double = 0.0): Double {
    require(amount.isFinite() && amount >= 0.0)
    require(discount.isFinite() && discount in 0.0..1.0)
    return amount * (1.0 - discount)
}

fun formatPrice(
    amount: Double,
    currency: String = "UAH",
    digits: Int = 2
): String {
    require(amount.isFinite())
    require(currency in setOf("UAH", "EUR", "USD"))
    require(digits in 0..4)
    return String.format(
        Locale.ROOT, "%.${digits}f %s", amount, currency
    )
}

fun main() {
    println(formatPrice(discountedPrice(1250.5)))
    val sale = discountedPrice(amount = 1000.0, discount = 0.15)
    println(formatPrice(sale, digits = 0))
    println(formatPrice(amount = 12.5, currency = "EUR", digits = 1))
    check(discountedPrice(10.0, discount = 1.0) == 0.0)
}
