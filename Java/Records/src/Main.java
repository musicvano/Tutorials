sealed interface Shape permits Circle, Rectangle { }
record Circle(double radius) implements Shape {
    Circle { Dimensions.check(radius); }
}
record Rectangle(double width, double height) implements Shape {
    Rectangle {
        Dimensions.check(width);
        Dimensions.check(height);
    }
}
final class Dimensions {
    private Dimensions() { }
    static void check(double value) {
        if (!Double.isFinite(value) || value <= 0 || value > 10_000) {
            throw new IllegalArgumentException("Invalid dimension");
        }
    }
}
public class Main {
    static double area(Shape shape) {
        return switch (shape) {
            case null -> throw new IllegalArgumentException(
                    "No shape");
            case Circle(double r) -> Math.PI * r * r;
            case Rectangle(double w, double h) -> w * h;
        };
    }
    static String kind(Shape shape) {
        return switch (shape) {
            case null -> "absent";
            case Circle c when c.radius() < 1 -> "small circle";
            case Circle _ -> "circle";
            case Rectangle _ -> "rectangle";
        };
    }
    public static void main(String[] args) {
        Shape[] shapes = {new Circle(0.5), new Rectangle(3, 4)};
        for (Shape shape : shapes) {
            System.out.printf(java.util.Locale.ROOT, "%s %.2f%n",
                    kind(shape), area(shape));
        }
        System.out.println(kind(null));
        try {
            new Circle(Double.NaN);
        } catch (IllegalArgumentException ex) {
            System.out.println(ex.getMessage());
        }
    }
}
