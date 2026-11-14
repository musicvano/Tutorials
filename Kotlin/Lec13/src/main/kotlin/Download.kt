import kotlinx.coroutines.*

suspend fun download(): String {
    try {
        delay(500)
        return "дані"
    } finally {
        println("Ресурс закрито")
    }
}

fun main() = runBlocking {
    val result = withTimeoutOrNull(80) { download() }
    println(result ?: "Час вичерпано")
    val fast = withTimeoutOrNull(500) {
        delay(1)
        "готово"
    }
    check(fast == "готово")
    println(fast)
}
