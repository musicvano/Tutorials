// Screenshot 15/04 (staging, not a lecture listing): IDE preview of the
// stateless TemperatureInput from Main.kt (example 2) with sample data.
import androidx.compose.material3.MaterialTheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.tooling.preview.Preview

@Preview
@Composable
fun TemperatureInputPreview() {
    MaterialTheme { TemperatureInput("36,6", onChange = {}) }
}
