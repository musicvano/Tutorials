package demo;

import java.sql.*;
import java.util.List;

public final class ReadersMain {
    interface ReaderDao {
        long add(String name) throws SQLException;
        void addAll(List<String> names) throws SQLException;
    }

    static final class JdbcReaders implements ReaderDao {
        private final Connection connection;
        JdbcReaders(Connection connection) {
            this.connection = connection;
        }

        public long add(String name) throws SQLException {
            try (PreparedStatement insert =
                    connection.prepareStatement(
                    "INSERT INTO j14_readers(name) VALUES (?)",
                    new String[]{"id"})) {
                insert.setString(1, name);
                insert.executeUpdate();
                try (ResultSet keys = insert.getGeneratedKeys()) {
                    if (!keys.next()) {
                        throw new SQLException("No key");
                    }
                    return keys.getLong(1);
                }
            }
        }

        public void addAll(List<String> names) throws SQLException {
            try (PreparedStatement insert =
                    connection.prepareStatement(
                    "INSERT INTO j14_readers(name) VALUES (?)")) {
                for (String name : names) {
                    insert.setString(1, name);
                    insert.addBatch();
                }
                insert.executeBatch();
            }
        }
    }

    public static void main(String[] args) throws Exception {
        try (Connection connection = Db.open();
             Statement setup = connection.createStatement()) {
            setup.executeUpdate("""
                CREATE TEMP TABLE j14_readers (
                  id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                  name text NOT NULL CHECK (length(trim(name)) > 0))
                """);
            connection.setAutoCommit(false);
            try {
                ReaderDao dao = new JdbcReaders(connection);
                System.out.println("id: " + dao.add("Олена"));
                dao.addAll(List.of("Тарас", "Марія"));
                connection.commit();
            } catch (SQLException error) {
                connection.rollback();
                throw error;
            }
            try (ResultSet rows = setup.executeQuery(
                    "SELECT count(*) FROM j14_readers")) {
                rows.next();
                System.out.println("Читачів: " + rows.getLong(1));
            }
        }
    }
}
