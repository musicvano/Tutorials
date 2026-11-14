sealed interface OrderStatus

data object Created : OrderStatus
data class Paid(val cents: Long) : OrderStatus {
    init { require(cents > 0) }
}
data class Shipped(val trackNo: String) : OrderStatus {
    init { require(trackNo.isNotBlank()) }
}
data class Cancelled(val reason: String) : OrderStatus {
    init { require(reason.isNotBlank()) }
}

fun describe(status: OrderStatus): String = when (status) {
    Created -> "created"
    is Paid -> "paid ${status.cents} kop"
    is Shipped -> "shipped ${status.trackNo}"
    is Cancelled -> "cancelled: ${status.reason}"
}

fun ship(status: OrderStatus, track: String): OrderStatus {
    require(status is Paid) { "Payment required" }
    return Shipped(track)
}

fun main() {
    val paid: OrderStatus = Paid(12500)
    val shipped = ship(paid, "STUDY-001")
    for (state in arrayOf(Created, paid, shipped)) {
        println(describe(state))
    }
    try {
        ship(Created, "STUDY-002")
    } catch (e: IllegalArgumentException) {
        println(e.message)
    }
    println(describe(paid))
}
