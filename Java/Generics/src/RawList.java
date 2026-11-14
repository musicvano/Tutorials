import java.util.ArrayList;
import java.util.List;

public class RawList {
    public static void main(String[] args) {
        List<String> names = new ArrayList<>();
        List raw = names;
        raw.add(42);
        String first = names.get(0);
        System.out.println(first);
    }
}
