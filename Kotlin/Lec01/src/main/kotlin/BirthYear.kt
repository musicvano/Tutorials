fun main() {
    print("Ім’я: ")
    val name = readln()
    print("Рік народження: ")
    val birthYear = readln().toInt()
    val referenceYear = 2026
    val ageAtYearEnd = referenceYear - birthYear
    println("Вітаю, $name!")
    println("Наприкінці $referenceYear: $ageAtYearEnd років")
}
