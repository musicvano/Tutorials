package demo.dao;

import demo.model.Book;
import java.sql.SQLException;
import java.util.List;

public interface BookRepository {
    List<Book> all() throws SQLException;
    void save(long id, String title, int year) throws SQLException;
    void delete(long id) throws SQLException;
}
