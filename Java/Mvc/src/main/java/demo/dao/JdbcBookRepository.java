package demo.dao;

import demo.model.Book;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public final class JdbcBookRepository implements BookRepository {
    Connection connect() throws SQLException {
        String url = System.getenv("COURSE_DB_URL");
        if (url == null) throw new SQLException("Missing DB URL");
        return DriverManager.getConnection(url,
            System.getenv("COURSE_DB_USER"),
            System.getenv("COURSE_DB_PASSWORD"));
    }

    @Override public List<Book> all() throws SQLException {
        List<Book> rows = new ArrayList<>();
        try (Connection c = connect();
             PreparedStatement p = c.prepareStatement(
                "SELECT id,title,year FROM j16_books "
                    + "ORDER BY id")) {
            p.setQueryTimeout(10);
            try (ResultSet r = p.executeQuery()) {
                while (r.next()) rows.add(new Book(r.getLong(1),
                    r.getString(2), r.getInt(3)));
            }
        }
        return rows;
    }

    @Override public void save(long id, String title, int year)
            throws SQLException {
        String sql = id == 0
            ? "INSERT INTO j16_books(title,year) VALUES (?,?)"
            : "UPDATE j16_books SET title=?,year=? WHERE id=?";
        try (Connection c = connect();
             PreparedStatement p = c.prepareStatement(sql)) {
            p.setQueryTimeout(10);
            p.setString(1, title);
            p.setInt(2, year);
            if (id != 0) p.setLong(3, id);
            if (p.executeUpdate() != 1) {
                throw new SQLException("Unknown book id");
            }
        }
    }

    @Override public void delete(long id) throws SQLException {
        try (Connection c = connect();
             PreparedStatement p = c.prepareStatement(
                "DELETE FROM j16_books WHERE id=?")) {
            p.setQueryTimeout(10);
            p.setLong(1, id);
            if (p.executeUpdate() != 1) {
                throw new SQLException("Unknown book id");
            }
        }
    }
}
