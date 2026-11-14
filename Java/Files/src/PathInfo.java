import java.nio.file.Files;
import java.nio.file.Path;

public class PathInfo {
    public static void main(String[] args) {
        Path input = Path.of("input.txt");
        System.out.println("user.dir: " + System.getProperty("user.dir"));
        System.out.println("input:    " + input.toAbsolutePath());
        System.out.println("exists:   " + Files.exists(input));
    }
}
