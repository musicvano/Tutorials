import java.io.BufferedReader;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;
import java.util.List;

public class Main {
    record Item(String name, int count) {}

    static List<Item> read(Path path) throws IOException {
        List<Item> items = new ArrayList<>();
        try (BufferedReader reader = Files.newBufferedReader(
                path, StandardCharsets.UTF_8)) {
            if (!"name,count".equals(reader.readLine())) {
                throw new IOException("invalid header");
            }
            String line;
            int number = 1;
            while ((line = reader.readLine()) != null) {
                number++;
                String[] fields = line.split(",", -1);
                try {
                    if (fields.length != 2
                            || !fields[0].matches("[A-Za-z]+")) {
                        throw new IllegalArgumentException();
                    }
                    int count = Integer.parseInt(fields[1]);
                    if (count < 0) {
                        throw new IllegalArgumentException();
                    }
                    items.add(new Item(fields[0], count));
                } catch (IllegalArgumentException error) {
                    throw new IOException(
                        "invalid row " + number, error);
                }
            }
        }
        return items;
    }

    public static void main(String[] args) throws IOException {
        Path dir = Files.createTempDirectory("java12-csv-");
        Path input = dir.resolve("input.csv");
        Path output = dir.resolve("report.txt");
        try {
            Files.writeString(input, "name,count\nTea,3\nBread,2\n",
                StandardCharsets.UTF_8);
            int total = 0;
            for (Item item : read(input)) {
                total = Math.addExact(total, item.count());
            }
            Files.writeString(output, "total: " + total + "\n",
                StandardCharsets.UTF_8);
            System.out.print(Files.readString(output));
        } finally {
            Files.deleteIfExists(output);
            Files.deleteIfExists(input);
            Files.deleteIfExists(dir);
        }
    }
}
