package ua.edu.app

import ua.edu.bank.Wallet as StudyWallet

fun main() {
    val wallet = StudyWallet("Taras", 1250)
    println(wallet.label())
}
