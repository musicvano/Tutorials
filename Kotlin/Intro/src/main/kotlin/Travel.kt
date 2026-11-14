fun main() {
    val distanceKm = 150.0
    val speedKmH = 60.0
    val totalMinutes = (distanceKm / speedKmH * 60).toInt()
    val hours = totalMinutes / 60
    val minutes = totalMinutes % 60
    println("Час: $hours год $minutes хв")
}
