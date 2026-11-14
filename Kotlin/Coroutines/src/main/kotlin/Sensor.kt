import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*

fun readings(): Flow<Int> = flow {
    for (value in listOf(-2, 0, 20)) {
        delay(5)
        emit(value)
    }
}

fun main() = runBlocking {
    val current = MutableStateFlow<Double?>(null)
    readings()
        .filter { it >= 0 }
        .map { it * 9.0 / 5 + 32 }
        .flowOn(Dispatchers.Default)
        .collect {
            current.value = it
            println("F = $it")
        }
    println("Останнє: ${current.value}")
    check(current.value == 68.0)
}
