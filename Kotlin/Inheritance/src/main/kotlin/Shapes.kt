import kotlin.math.PI
import kotlin.math.sqrt
import java.util.Locale

interface Drawable {
    fun draw(): String
}

abstract class Shape {
    abstract fun area(): Double
    abstract fun perimeter(): Double
}

fun positive(value: Double): Double {
    require(value.isFinite() && value in 0.001..10000.0)
    return value
}

class Circle(radius: Double) : Shape(), Drawable {
    private val radius = positive(radius)
    override fun area(): Double = PI * radius * radius
    override fun perimeter(): Double = 2 * PI * radius
    override fun draw(): String = "Circle"
}

open class Rectangle(width: Double, height: Double) :
    Shape(), Drawable {
    private val width = positive(width)
    private val height = positive(height)
    override fun area(): Double = width * height
    override fun perimeter(): Double = 2 * (width + height)
    override fun draw(): String = "Rectangle"
}

class Square(side: Double) : Rectangle(side, side)

class Triangle(a: Double, b: Double, c: Double) : Shape() {
    private val a = positive(a)
    private val b = positive(b)
    private val c = positive(c)
    init {
        require(a + b > c && a + c > b && b + c > a)
    }
    override fun perimeter(): Double = a + b + c
    override fun area(): Double {
        val s = perimeter() / 2
        return sqrt(s * (s - a) * (s - b) * (s - c))
    }
}

fun main() {
    val shapes: Array<Shape> = arrayOf(
        Circle(1.0), Rectangle(3.0, 4.0),
        Square(2.0), Triangle(3.0, 4.0, 5.0)
    )
    for (shape in shapes) {
        println(String.format(
            Locale.ROOT, "%.2f %.2f",
            shape.area(), shape.perimeter()
        ))
    }
}
