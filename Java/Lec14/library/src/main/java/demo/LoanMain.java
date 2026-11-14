package demo;

import java.sql.*;

public final class LoanMain {
    static void issue(Connection connection, long book,
                      String reader, boolean fail)
            throws SQLException {
        connection.setAutoCommit(false);
        try {
            try (PreparedStatement update =
                    connection.prepareStatement(
                    "UPDATE j14_stock SET available=false " +
                    "WHERE id=? AND available=true")) {
                update.setLong(1, book);
                if (update.executeUpdate() != 1) {
                    throw new SQLException(
                        "Book unavailable", "P0001");
                }
            }
            if (fail) throw new SQLException("Simulated failure");
            try (PreparedStatement insert =
                    connection.prepareStatement(
                    "INSERT INTO j14_loans(book_id,reader) "
                    + "VALUES (?,?)")) {
                insert.setLong(1, book);
                insert.setString(2, reader);
                insert.executeUpdate();
            }
            connection.commit();
        } catch (SQLException error) {
            try { connection.rollback(); }
            catch (SQLException rollback) {
                error.addSuppressed(rollback);
            }
            throw error;
        }
    }

    static boolean available(Connection connection)
            throws SQLException {
        try (Statement statement = connection.createStatement();
             ResultSet rows = statement.executeQuery(
                 "SELECT available FROM j14_stock WHERE id=1")) {
            if (!rows.next()) throw new SQLException("Missing book");
            return rows.getBoolean(1);
        }
    }

    public static void main(String[] args) throws Exception {
        try (Connection connection = Db.open();
             Statement setup = connection.createStatement()) {
            setup.executeUpdate("""
                CREATE TEMP TABLE j14_stock (
                  id bigint PRIMARY KEY, available boolean NOT NULL)
                """);
            setup.executeUpdate("""
                CREATE TEMP TABLE j14_loans (
                  book_id bigint REFERENCES j14_stock(id),
                  reader text)
                """);
            setup.executeUpdate(
                "INSERT INTO j14_stock VALUES (1,true)");
            try { issue(connection, 1, "Олена", true); }
            catch (SQLException expected) {
                System.out.println(
                    "Відкат: " + available(connection));
            }
            issue(connection, 1, "Олена", false);
            System.out.println(
                "Після видачі: " + available(connection));
        }
    }
}
