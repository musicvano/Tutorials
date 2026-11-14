import kotlinx.coroutines.*
import kotlin.time.measureTime

suspend fun tea(): String {
    delay(100)
    return "чай"
}

suspend fun toast(): String {
    delay(150)
    return "тост"
}

suspend fun breakfast(): List<String> = coroutineScope {
    val drink = async { tea() }
    val food = async { toast() }
    listOf(drink.await(), food.await())
}

fun main() = runBlocking {
    var sequential: List<String> = emptyList()
    val first = measureTime {
        sequential = listOf(tea(), toast())
    }
    var concurrent: List<String> = emptyList()
    val second = measureTime { concurrent = breakfast() }
    check(sequential == concurrent)
    println(concurrent.joinToString(" + "))
    println("Послідовно: $first")
    println("Конкурентно: $second")
}
