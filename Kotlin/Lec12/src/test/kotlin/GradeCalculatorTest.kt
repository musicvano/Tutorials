import kotlin.test.Test
import kotlin.test.assertEquals
import kotlin.test.assertFailsWith
import kotlin.test.assertTrue
import org.junit.jupiter.params.ParameterizedTest
import org.junit.jupiter.params.provider.CsvSource

class GradeCalculatorTest {
    @ParameterizedTest
    @CsvSource("0,false", "59,false", "60,true", "100,true")
    fun boundaries(score: Int, expected: Boolean) {
        assertEquals(expected, passed(score))
    }

    @Test
    fun mean() {
        val input = listOf(50, 70, 90)
        val result = averageScore(input)
        assertEquals(70.0, result)
        assertTrue(passed(result.toInt()))
    }

    @Test
    fun invalid() {
        assertFailsWith<IllegalArgumentException> { passed(-1) }
        assertFailsWith<IllegalArgumentException> { passed(101) }
        assertFailsWith<IllegalArgumentException> {
            averageScore(emptyList())
        }
    }
}
