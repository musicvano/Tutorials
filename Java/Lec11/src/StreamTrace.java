import java.util.List;

public class StreamTrace {
    public static void main(String[] args) {
        List<String> words = List.of(
            "tea", "bread", "milk", "jam", "apple", "butter");
        List<String> result = words.stream()
            .filter(w -> w.length() > 3)
            .map(String::toUpperCase)
            .limit(2)
            .toList();
        System.out.println(result);
    }
}
