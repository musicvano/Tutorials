fun main() {
    try {
        print("Ділене: ")
        val a = readlnOrNull()?.toInt()
            ?: throw IllegalArgumentException("Немає діленого")
        print("Дільник: ")
        val b = readlnOrNull()?.toInt()
            ?: throw IllegalArgumentException("Немає дільника")
        require(a in -1000000..1000000)
        require(b in -1000000..1000000)
        println("Частка: ${a / b}")
        println("Остача: ${a % b}")
    } catch (error: NumberFormatException) {
        println("Потрібне ціле число")
    } catch (error: ArithmeticException) {
        println("Ділення на нуль")
    } catch (error: IllegalArgumentException) {
        println("Неправильний ввід: ${error.message}")
    } finally {
        println("Спробу завершено")
    }
}
