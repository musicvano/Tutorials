import java.util.Arrays;
import java.util.Objects;

final class Point {
    private final int x;
    private final int y;
    Point(int x, int y) {
        this.x = x;
        this.y = y;
    }
    Point(Point other) {
        this(Objects.requireNonNull(other).x, other.y);
    }
    @Override
    public boolean equals(Object other) {
        if (this == other) { return true; }
        return other instanceof Point point
                && x == point.x && y == point.y;
    }
    @Override
    public int hashCode() { return Objects.hash(x, y); }
    @Override
    public String toString() { return "(" + x + ", " + y + ")"; }
}

public class Main {
    public static void main(String[] args) {
        Point a = new Point(2, 3);
        Point b = new Point(a);
        System.out.println(a == b);
        System.out.println(a.equals(b));
        System.out.println(a.hashCode() == b.hashCode());
        System.out.println(a.equals(null));
        System.out.println(a.equals("(2, 3)"));
        Point[] first = {a};
        Point[] second = {b};
        System.out.println(first.equals(second));
        System.out.println(Arrays.equals(first, second));
        System.out.println(b);
    }
}
