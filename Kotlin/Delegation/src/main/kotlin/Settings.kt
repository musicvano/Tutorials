import kotlin.properties.Delegates

class Settings(private val values: Map<String, Any>) {
    val title: String by values
    val width: Int by values

    val summary: String by lazy {
        println("compute summary")
        "$title/$width"
    }

    var theme: String by Delegates.observable("light") {
        _, old, new -> println("theme: $old -> $new")
    }

    var volume: Int by Delegates.vetoable(50) {
        _, _, new -> new in 0..100
    }
}

fun main() {
    val settings = Settings(mapOf("title" to "Study", "width" to 80))
    println(settings.summary)
    println(settings.summary)
    settings.theme = "dark"
    settings.volume = 150
    println("volume=${settings.volume}")
    settings.volume = 75
    println("volume=${settings.volume}")
}
