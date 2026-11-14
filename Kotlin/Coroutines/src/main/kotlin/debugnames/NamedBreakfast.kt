// Screenshot 13/05 (staging, not a lecture listing): example 1 with the
// CoroutineName the lecture recommends and the thread name in each line;
// run with the VM option -Dkotlinx.coroutines.debug. Own package so tea()
// and toast() do not clash with Breakfast.kt.
package debugnames

import kotlinx.coroutines.*

fun log(message: String) =
    println("[${Thread.currentThread().name}] $message")

suspend fun tea(): String {
    delay(100)
    log("чай готовий")
    return "чай"
}

suspend fun toast(): String {
    delay(150)
    log("тост готовий")
    return "тост"
}

fun main() = runBlocking(CoroutineName("breakfast")) {
    val drink = async(CoroutineName("tea")) { tea() }
    val food = async(CoroutineName("toast")) { toast() }
    log(listOf(drink.await(), food.await()).joinToString(" + "))
}
