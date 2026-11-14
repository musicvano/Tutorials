import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.*
import androidx.navigation.compose.*
import androidx.navigation.toRoute
import kotlinx.serialization.Serializable

@Serializable object NoteList
@Serializable data class NoteDetail(val id: Int)

@Composable
fun NotesNavigation() {
    val notes = mapOf(1 to "Підготувати звіт", 2 to "Прочитати тему")
    val navigation = rememberNavController()
    NavHost(navigation, startDestination = NoteList) {
        composable<NoteList> {
            Column(Modifier.padding(20.dp)) {
                Text("Нотатки")
                for ((id, title) in notes) {
                    Button(onClick = {
                        navigation.navigate(NoteDetail(id))
                    }) { Text(title) }
                }
            }
        }
        composable<NoteDetail> { entry ->
            val route = entry.toRoute<NoteDetail>()
            Column(Modifier.padding(20.dp)) {
                Text(notes[route.id] ?: "Нотатку не знайдено")
                Button(onClick = { navigation.popBackStack() }) {
                    Text("Назад")
                }
            }
        }
    }
}

fun main() = application {
    Window(onCloseRequest = ::exitApplication,
        title = "Навігація нотаток") {
        MaterialTheme { NotesNavigation() }
    }
}
