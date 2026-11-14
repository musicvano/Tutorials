package ua.knu.loan;

import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import static org.junit.jupiter.api.Assertions.*;

class NameValidatorTest {
    @ParameterizedTest
    @CsvSource({"'  Олена  ', Олена", "Тарас, Тарас", "AB, AB"})
    void trimsValidNames(String input, String expected) {
        assertEquals(expected, NameValidator.normalize(input));
    }

    @Nested
    class Invalid {
        @Test
        void rejectsMissing() {
            assertThrows(IllegalArgumentException.class,
                () -> NameValidator.normalize(null));
        }
        @Test
        void rejectsShort() {
            assertThrows(IllegalArgumentException.class,
                () -> NameValidator.normalize(" A "));
        }
    }
}
