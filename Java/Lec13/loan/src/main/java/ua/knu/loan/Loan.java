package ua.knu.loan;

public final class Loan {
    private Loan() {}

    public static double payment(double sum, double annual,
                                 int months) {
        if (!Double.isFinite(sum) || !Double.isFinite(annual)
                || sum <= 0 || annual < 0 || annual > 100
                || months < 1 || months > 600) {
            throw new IllegalArgumentException("Invalid loan");
        }
        double rate = annual / 1200;
        if (rate == 0) return sum / months;
        double divisor = -Math.expm1(-months * Math.log1p(rate));
        double result = sum * rate / divisor;
        if (!Double.isFinite(result)) {
            throw new ArithmeticException("Payment overflow");
        }
        return result;
    }

    public static void main(String[] args) {
        System.out.printf(java.util.Locale.ROOT, "%.2f%n",
            payment(12000, 0, 12));
    }
}
