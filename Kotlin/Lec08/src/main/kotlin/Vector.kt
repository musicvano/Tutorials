import kotlin.math.hypot

data class Vector(val x: Double, val y: Double) :
    Comparable<Vector> {
    init {
        require(x.isFinite() && y.isFinite())
        require(x in -1e6..1e6 && y in -1e6..1e6)
    }
    val length: Double get() = hypot(x, y)

    operator fun plus(other: Vector): Vector =
        Vector(x + other.x, y + other.y)

    operator fun times(scale: Double): Vector {
        require(scale.isFinite())
        return Vector(x * scale, y * scale)
    }

    operator fun unaryMinus(): Vector = Vector(-x, -y)

    operator fun get(index: Int): Double = when (index) {
        0 -> x
        1 -> y
        else -> throw IndexOutOfBoundsException("index=$index")
    }

    override operator fun compareTo(other: Vector): Int =
        length.compareTo(other.length)
}

fun main() {
    val a = Vector(3.0, 4.0)
    val b = Vector(1.0, 2.0)
    println(a + b)
    println(a * 2.0)
    println(-b)
    println("x=${a[0]}; length=${a.length}")
    println(a > b)
    val rotated = Vector(4.0, 3.0)
    println("order=${a.compareTo(rotated)}; equal=${a == rotated}")
}
