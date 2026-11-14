package demo;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public final class CatalogMain {
    record Book(long id, String title, String author) {}

    static List<Book> find(Connection connection, String author)
            throws SQLException {
        String sql = """
            SELECT b.id, b.title, a.name AS author
            FROM j14_books b JOIN j14_authors a ON a.id=b.author_id
            WHERE a.name = ? ORDER BY b.id
            """;
        List<Book> books = new ArrayList<>();
        try (PreparedStatement query =
                connection.prepareStatement(sql)) {
            query.setString(1, author);
            try (ResultSet rows = query.executeQuery()) {
                while (rows.next()) {
                    books.add(new Book(rows.getLong("id"),
                        rows.getString("title"),
                        rows.getString("author")));
                }
            }
        }
        return List.copyOf(books);
    }

    public static void main(String[] args) throws Exception {
        try (Connection connection = Db.open();
             Statement setup = connection.createStatement()) {
            setup.executeUpdate("""
                CREATE TEMP TABLE j14_authors (
                  id bigint PRIMARY KEY, name text NOT NULL)
                """);
            setup.executeUpdate("""
                CREATE TEMP TABLE j14_books (
                  id bigint PRIMARY KEY, title text NOT NULL,
                  author_id bigint REFERENCES j14_authors(id))
                """);
            setup.executeUpdate("""
                INSERT INTO j14_authors VALUES (1, 'Автор A')
                """);
            setup.executeUpdate("""
                INSERT INTO j14_books VALUES
                  (1, 'Весна', 1), (2, 'Літо', 1)
                """);
            find(connection, "Автор A").forEach(System.out::println);
            System.out.println("Невідомий: " +
                find(connection, "' OR '1'='1").size());
        }
    }
}
