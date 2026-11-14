package demo;

import demo.dao.BookRepository;
import demo.dao.SlowBookRepository;
import java.time.Duration;

// Demo launcher for screenshot 16/13: the FXML application with
// the in-memory demo DAO that waits 5 s before every operation.
public class SlowBookApp extends BookApp {
    @Override protected BookRepository repository() {
        return new SlowBookRepository(Duration.ofSeconds(5));
    }

    public static void main(String[] args) { launch(args); }
}
