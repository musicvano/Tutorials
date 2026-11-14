import kotlinx.coroutines.*
import kotlinx.coroutines.sync.Mutex
import kotlinx.coroutines.sync.withLock

class Account {
    private val mutex = Mutex()
    private var cents = 0L

    suspend fun deposit(amount: Long) {
        require(amount > 0)
        mutex.withLock {
            cents = Math.addExact(cents, amount)
        }
    }

    suspend fun balance(): Long = mutex.withLock { cents }
}

fun main() = runBlocking {
    val account = Account()
    List(20) {
        launch(Dispatchers.Default) {
            repeat(100) { account.deposit(1) }
        }
    }.joinAll()
    check(account.balance() == 2000L)
    println("Баланс: ${account.balance()} коп.")
}
