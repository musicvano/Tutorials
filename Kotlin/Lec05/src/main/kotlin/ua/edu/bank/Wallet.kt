package ua.edu.bank

internal class Wallet(val owner: String, val cents: Long) {
    init {
        require(owner.isNotBlank() && cents >= 0)
    }

    fun label(): String = "$owner: $cents kop"
}
