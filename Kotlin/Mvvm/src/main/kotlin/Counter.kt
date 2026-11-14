import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.*
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewmodel.compose.viewModel
import kotlinx.coroutines.flow.*

class CounterViewModel : ViewModel() {
    private val mutable = MutableStateFlow(0)
    val count = mutable.asStateFlow()
    fun increment() { mutable.update { (it + 1).coerceAtMost(100) } }
    fun reset() { mutable.value = 0 }
}

@Composable
fun CounterScreen(model: CounterViewModel = viewModel {
    CounterViewModel()
}) {
    val count by model.count.collectAsState()
    Column(Modifier.padding(20.dp)) {
        Text("Кількість: $count")
        Button(onClick = model::increment, enabled = count < 100) {
            Text("Додати")
        }
        Button(onClick = model::reset) { Text("Скинути") }
    }
}

fun main() = application {
    Window(onCloseRequest = ::exitApplication, title = "MVVM") {
        MaterialTheme { CounterScreen() }
    }
}
