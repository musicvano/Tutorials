import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.TreeMap;

public class Main {
    public static void main(String[] args) {
        String text = "Tea, bread; tea. Milk bread tea";
        Map<String, Integer> counts = new HashMap<>();
        for (String word : text.toLowerCase(Locale.ROOT)
                .split("[^a-z]+")) {
            if (!word.isEmpty()) {
                counts.merge(word, 1, Integer::sum);
            }
        }
        Map<String, Integer> ordered = new TreeMap<>(counts);
        int total = 0;
        for (Map.Entry<String, Integer> entry : ordered.entrySet()) {
            System.out.println(entry.getKey()
                + ": " + entry.getValue());
            total += entry.getValue();
        }
        System.out.println("total: " + total);
    }
}
