import java.util.Locale;

public class FunctionTable {
    public static void main(String[] args) {
        double total = 0;
        System.out.printf("%5s %10s%n", "x", "y");
        for (int x = -2; x <= 2; x++) {
            double y = (double) x * x - 2 * x + 1;
            total += y;
            System.out.printf(Locale.US, "%5d %10.2f%n", x, y);
        }
        System.out.printf(Locale.US, "Total: %.2f%n", total);
    }
}
