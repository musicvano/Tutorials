fun main(args: Array<String>) {
    if (args.size != 2) {
        println("Потрібні аргументи: ім’я група")
        return
    }
    println("Ім’я: ${args[0]}")
    println("Група: ${args[1]}")
}
