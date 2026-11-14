package demo.dao;

import demo.model.Book;
import java.sql.SQLException;
import java.time.Duration;
import java.util.ArrayList;
import java.util.List;

// Demo DAO for screenshot 16/13 only: synthetic rows in memory and
// a controlled delay before every operation. Not the working DAO.
public final class SlowBookRepository implements BookRepository {
    private final Duration delay;
    private final List<Book> rows = new ArrayList<>(List.of(
        new Book(1, "Java", 2026), new Book(2, "SQL", 2024),
        new Book(3, "JavaFX", 2025)));
    private long nextId = 4;

    public SlowBookRepository(Duration delay) {
        this.delay = delay;
    }

    private void pause() throws SQLException {
        try {
            Thread.sleep(delay);
        } catch (InterruptedException error) {
            Thread.currentThread().interrupt();
            throw new SQLException("Interrupted", error);
        }
    }

    @Override public synchronized List<Book> all()
            throws SQLException {
        pause();
        return List.copyOf(rows);
    }

    @Override public synchronized void save(long id, String title,
            int year) throws SQLException {
        pause();
        if (id == 0) {
            rows.add(new Book(nextId++, title, year));
            return;
        }
        for (int i = 0; i < rows.size(); i++) {
            if (rows.get(i).id() == id) {
                rows.set(i, new Book(id, title, year));
                return;
            }
        }
        throw new SQLException("Unknown book id");
    }

    @Override public synchronized void delete(long id)
            throws SQLException {
        pause();
        if (!rows.removeIf(book -> book.id() == id)) {
            throw new SQLException("Unknown book id");
        }
    }
}
