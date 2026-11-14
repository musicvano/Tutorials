import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.*

@Composable
fun Counter() {
    var count by remember { mutableStateOf(0) }
    Column(Modifier.padding(20.dp)) {
        Text("Кількість: $count")
        Row(horizontalArrangement = Arrangement.spacedBy(8.dp)) {
            Button(onClick = { count++ }, enabled = count < 100) {
                Text("Додати")
            }
            Button(onClick = { count = 0 }) { Text("Скинути") }
        }
    }
}

fun main() = application {
    Window(onCloseRequest = ::exitApplication,
        title = "Лічильник") {
        MaterialTheme { Counter() }
    }
}
