data class Order(
    val id: Int,
    val city: String,
    val cents: Long,
    val paid: Boolean
)

fun totals(orders: List<Order>): List<Pair<String, Long>> {
    require(orders.all { it.cents in 0..1_000_000 })
    require(orders.size <= 100_000)
    require(orders.map { it.id }.toSet().size == orders.size)
    return orders.filter { it.paid }
        .groupBy { it.city }
        .map { (city, values) -> city to values.sumOf { it.cents } }
        .sortedWith(
            compareByDescending<Pair<String, Long>> { it.second }
                .thenBy { it.first }
        )
}

fun main() {
    val orders = listOf(
        Order(1, "London", 500, true),
        Order(2, "Madrid", 700, true),
        Order(3, "London", 200, true),
        Order(4, "Paris", 900, false)
    )
    println(totals(orders))
    println(totals(emptyList()))
    println(orders.groupingBy { it.paid }.eachCount())
    println(orders.fold(0L) { sum, order -> sum + order.cents })
}
