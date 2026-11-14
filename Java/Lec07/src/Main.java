import java.util.Arrays;
import java.util.Comparator;
import java.util.Objects;

final class Product implements Comparable<Product> {
    private final String name;
    private final long price;
    Product(String name, long price) {
        if (name == null || name.isBlank()
                || price < 0 || price > 1_000_000) {
            throw new IllegalArgumentException("Invalid product");
        }
        this.name = name.strip();
        this.price = price;
    }
    public String name() { return name; }
    public long price() { return price; }
    @Override
    public int compareTo(Product other) {
        int byName = name.compareTo(other.name);
        return byName != 0 ? byName
                : Long.compare(price, other.price);
    }
    @Override
    public boolean equals(Object other) {
        return other instanceof Product p
                && name.equals(p.name) && price == p.price;
    }
    @Override
    public int hashCode() { return Objects.hash(name, price); }
    @Override
    public String toString() { return name + ":" + price; }
}
public class Main {
    public static void main(String[] args) {
        Product[] items = {
            new Product("Pen", 500),
            new Product("Book", 2000),
            new Product("Pen", 300)
        };
        Arrays.sort(items);
        System.out.println(Arrays.toString(items));
        Comparator<Product> byPrice = new Comparator<Product>() {
            @Override
            public int compare(Product a, Product b) {
                int result = Long.compare(a.price(), b.price());
                return result != 0 ? result
                        : a.name().compareTo(b.name());
            }
        };
        Arrays.sort(items, byPrice);
        System.out.println(Arrays.toString(items));
        System.out.println(new Product("Pen", 500)
                .compareTo(new Product("Pen", 500)));
    }
}
