package ua.knu.loan;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class LoanTest {
    @Test
    void zeroRate() {
        double actual = Loan.payment(12000, 0, 12);
        assertEquals(1000, actual, 0.000001);
    }

    @Test
    void rejectsInvalidInput() {
        assertAll(
            () -> assertThrows(IllegalArgumentException.class,
                () -> Loan.payment(-1, 10, 12)),
            () -> assertThrows(IllegalArgumentException.class,
                () -> Loan.payment(1000, Double.NaN, 12)),
            () -> assertThrows(IllegalArgumentException.class,
                () -> Loan.payment(1000, 10, 0)),
            () -> assertThrows(ArithmeticException.class,
                () -> Loan.payment(Double.MAX_VALUE, 100, 1))
        );
    }
}
