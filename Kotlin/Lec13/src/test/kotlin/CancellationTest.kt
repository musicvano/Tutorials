import kotlin.test.Test
import kotlin.test.assertEquals
import kotlin.test.assertNull
import kotlin.test.assertTrue
import kotlinx.coroutines.delay
import kotlinx.coroutines.withTimeoutOrNull
import kotlinx.coroutines.test.runTest

class CancellationTest {
    @Test
    fun timeoutClosesResource() = runTest {
        var closed = false
        val result = withTimeoutOrNull(10) {
            try {
                delay(1000)
                "готово"
            } finally {
                closed = true
            }
        }
        assertNull(result)
        assertTrue(closed)
    }

    @Test
    fun quickOperationCompletes() = runTest {
        val result = withTimeoutOrNull(1000) {
            delay(10)
            "готово"
        }
        assertEquals("готово", result)
    }
}
