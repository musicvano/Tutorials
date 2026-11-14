import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;

public class CatalogService {
    private final List<Product> items = new ArrayList<>();
    private Product[] sortedCache;

    public void add(Product product) {
        if (product == null) {
            throw new IllegalArgumentException("No product");
        }
        items.add(product);
        sortedCache = null;
    }

    public Optional<Product> find(String name) {
        for (Product product : items) {
            if (product.name().equals(name)) {
                return Optional.of(product);
            }
        }
        return Optional.empty();
    }

    public Product[] sorted() {
        if (sortedCache == null) {
            sortedCache = items.toArray(new Product[0]);
            Arrays.sort(sortedCache);
        }
        return sortedCache.clone();
    }

    public void clearCache() {
        sortedCache = null;
    }
}
