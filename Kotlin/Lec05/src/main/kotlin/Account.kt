class Account(val number: String, initial: Long = 0) {
    var balance: Long = initial
        private set

    init {
        require(number.isNotBlank()) { "Empty number" }
        require(initial in 0..1_000_000_000L)
    }

    fun deposit(amount: Long) {
        require(amount > 0) { "Amount must be positive" }
        require(amount <= 1_000_000_000L - balance)
        balance += amount
    }

    fun withdraw(amount: Long) {
        require(amount > 0) { "Amount must be positive" }
        require(amount <= balance) { "Insufficient balance" }
        balance -= amount
    }

    override fun toString(): String = "$number: $balance"
}

fun main() {
    val a = Account("A1", 1000)
    val b = a
    val c = Account("A1", 1000)
    b.deposit(500)
    a.withdraw(200)
    println(a)
    println(c)
    println("same=${a === b}; equal=${a == c}")
    try {
        a.withdraw(2000)
    } catch (e: IllegalArgumentException) {
        println("Rejected: ${e.message}")
    }
    println("balance=${a.balance}")
}
