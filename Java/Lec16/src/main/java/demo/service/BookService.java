package demo.service;

import demo.dao.BookRepository;
import demo.model.Book;
import java.sql.SQLException;
import java.util.List;

public final class BookService {
    private final BookRepository repository;

    public BookService(BookRepository repository) {
        this.repository = repository;
    }

    public List<Book> all() throws SQLException {
        return repository.all();
    }

    public void save(long id, String title, int year)
            throws SQLException {
        if (id < 0 || title == null || title.isBlank()
                || title.length() > 100 || year < 1450
                || year > 2100) {
            throw new IllegalArgumentException("Invalid book");
        }
        repository.save(id, title.strip(), year);
    }

    public void delete(long id) throws SQLException {
        if (id <= 0) {
            throw new IllegalArgumentException("Select book");
        }
        repository.delete(id);
    }
}
