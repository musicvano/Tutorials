import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;

public class ReadText {
    public static void main(String[] args) throws IOException {
        Path input = Path.of("input.txt");
        for (String line : Files.readAllLines(input, StandardCharsets.UTF_8)) {
            int bytes = line.getBytes(StandardCharsets.UTF_8).length;
            System.out.println(line + " | chars: " + line.length()
                + ", bytes: " + bytes);
        }
    }
}
