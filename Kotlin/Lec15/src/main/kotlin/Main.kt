import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.*

fun fahrenheit(text: String): Double? {
    val c = text.replace(',', '.').toDoubleOrNull() ?: return null
    if (!c.isFinite() || c < -273.15) return null
    return (c * 9 / 5 + 32).takeIf { it.isFinite() }
}

@Composable
fun TemperatureInput(value: String, onChange: (String) -> Unit) {
    OutlinedTextField(value = value, onValueChange = onChange,
        label = { Text("Градуси Цельсія") },
        isError = value.isNotEmpty() && fahrenheit(value) == null)
}

@Composable
fun TemperatureScreen() {
    var value by remember { mutableStateOf("20") }
    val result = fahrenheit(value)
    Column(Modifier.padding(20.dp)) {
        TemperatureInput(value, onChange = { value = it })
        Text(result?.let { "$it °F" } ?: "Введіть коректне число")
    }
}

fun main() = application {
    Window(onCloseRequest = ::exitApplication,
        title = "Температура") {
        MaterialTheme { TemperatureScreen() }
    }
}
