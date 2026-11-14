import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.*

data class Task(val id: Int, val title: String, val done: Boolean)

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun Tasks() {
    val tasks = remember { mutableStateListOf<Task>() }
    var nextId by remember { mutableStateOf(1) }
    var dialog by remember { mutableStateOf(false) }
    var draft by remember { mutableStateOf("") }
    Scaffold(topBar = { TopAppBar(title = { Text("Справи") }) },
        floatingActionButton = {
            FloatingActionButton(onClick = { dialog = true }) {
                Text("+")
            }
        }) { padding ->
        LazyColumn(Modifier.padding(padding)) {
            items(tasks, key = { it.id }) { task ->
                Row(Modifier.padding(8.dp)) {
                    Checkbox(task.done, onCheckedChange = { done ->
                        val index = tasks.indexOfFirst {
                            it.id == task.id
                        }
                        if (index >= 0) {
                            tasks[index] = task.copy(done = done)
                        }
                    })
                    Text(task.title)
                }
            }
        }
    }
    if (dialog) AlertDialog(onDismissRequest = { dialog = false },
        title = { Text("Нова справа") },
        text = {
            OutlinedTextField(draft, onValueChange = { draft = it })
        },
        confirmButton = {
            TextButton(enabled = draft.isNotBlank(), onClick = {
                tasks.add(Task(nextId++, draft.trim(), false))
                draft = ""
                dialog = false
            }) { Text("Додати") }
        },
        dismissButton = {
            TextButton(onClick = { draft = ""; dialog = false }) {
                Text("Скасувати")
            }
        })
}

fun main() = application {
    Window(onCloseRequest = ::exitApplication, title = "Справи") {
        MaterialTheme { Tasks() }
    }
}
