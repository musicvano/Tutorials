import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json

@Serializable
sealed interface CatalogItem {
    val title: String
}

@Serializable
@SerialName("printed")
data class PrintedBook(
    override val title: String,
    val pages: Int
) : CatalogItem {
    init { require(title.isNotBlank() && pages > 0) }
}

@Serializable
@SerialName("audio")
data class AudioBook(
    override val title: String,
    val minutes: Int
) : CatalogItem {
    init { require(title.isNotBlank() && minutes > 0) }
}

fun main() {
    val format = Json {
        classDiscriminator = "kind"
        prettyPrint = true
    }
    val items: List<CatalogItem> = listOf(
        PrintedBook("Kobzar", 200),
        AudioBook("Forest", 90)
    )
    val text = format.encodeToString(items)
    println(text)
    val restored = format.decodeFromString<List<CatalogItem>>(text)
    println(restored == items)
    check(restored.size == 2)
}
