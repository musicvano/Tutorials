package demo;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.function.Consumer;
import javafx.application.Application;
import javafx.beans.binding.Bindings;
import javafx.beans.property.SimpleBooleanProperty;
import javafx.collections.FXCollections;
import javafx.concurrent.Task;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.HBox;
import javafx.scene.layout.VBox;
import javafx.stage.Stage;

public class BookMain extends Application {
    record Book(long id, String title, int year) {
        @Override public String toString() {
            return id + ": " + title + " (" + year + ")";
        }
    }

    interface Repository {
        List<Book> all() throws SQLException;
        void save(long id, String title, int year)
            throws SQLException;
        void delete(long id) throws SQLException;
    }

    static final class JdbcRepository implements Repository {
        Connection connect() throws SQLException {
            String url = System.getenv("COURSE_DB_URL");
            if (url == null) throw new SQLException("Missing DB URL");
            return DriverManager.getConnection(url,
                System.getenv("COURSE_DB_USER"),
                System.getenv("COURSE_DB_PASSWORD"));
        }

        public List<Book> all() throws SQLException {
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

        public void save(long id, String title, int year)
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

        public void delete(long id) throws SQLException {
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

    static final class Service {
        private final Repository repository;
        Service(Repository repository) {
            this.repository = repository;
        }
        List<Book> all() throws SQLException {
            return repository.all();
        }
        void save(long id, String title, int year)
                throws SQLException {
            if (id < 0 || title == null || title.isBlank()
                    || title.length() > 100 || year < 1450
                    || year > 2100) {
                throw new IllegalArgumentException("Invalid book");
            }
            repository.save(id, title.strip(), year);
        }
        void delete(long id) throws SQLException {
            if (id <= 0) {
                throw new IllegalArgumentException("Select book");
            }
            repository.delete(id);
        }
    }

    private final Service service = new Service(new JdbcRepository());
    private final ExecutorService worker =
        Executors.newSingleThreadExecutor(runnable -> {
            Thread thread = new Thread(runnable, "book-dao");
            thread.setDaemon(true);
            return thread;
        });
    private final SimpleBooleanProperty busy =
        new SimpleBooleanProperty(false);
    private final Label status = new Label("Ready");

    <T> void run(Callable<T> action, Consumer<T> success) {
        if (busy.get()) return;
        busy.set(true);
        status.setText("Busy");
        Task<T> task = new Task<>() {
            @Override protected T call() throws Exception {
                return action.call();
            }
        };
        task.setOnSucceeded(event -> {
            busy.set(false);
            success.accept(task.getValue());
        });
        task.setOnFailed(event -> {
            busy.set(false);
            status.setText("Operation failed; draft kept");
        });
        task.setOnCancelled(event -> {
            busy.set(false);
            status.setText("Cancelled; reload to verify DB state");
        });
        worker.execute(task);
    }

    @Override public void start(Stage stage) {
        ListView<Book> list = new ListView<>(
            FXCollections.observableArrayList());
        TextField title = new TextField();
        TextField year = new TextField("2026");
        Button load = new Button("Reload");
        Button add = new Button("Add");
        Button save = new Button("Save selected");
        Button delete = new Button("Delete selected");
        list.disableProperty().bind(busy);
        title.disableProperty().bind(busy);
        year.disableProperty().bind(busy);
        load.disableProperty().bind(busy);
        var invalid = Bindings.createBooleanBinding(
            () -> title.getText().isBlank(), title.textProperty());
        add.disableProperty().bind(busy.or(invalid));
        var absent = list.getSelectionModel()
            .selectedItemProperty().isNull();
        save.disableProperty().bind(busy.or(invalid).or(absent));
        delete.disableProperty().bind(busy.or(absent));
        list.getSelectionModel().selectedItemProperty()
            .addListener((value, oldBook, book) -> {
                if (book != null) {
                    title.setText(book.title());
                    year.setText(Integer.toString(book.year()));
                }
            });
        load.setOnAction(event -> run(service::all, rows -> {
            list.getItems().setAll(rows);
            status.setText("Loaded: " + rows.size());
        }));
        Consumer<Long> write = id -> {
            try {
                String text = title.getText();
                int number = Integer.parseInt(year.getText());
                run(() -> {
                    service.save(id, text, number);
                    return id;
                },
                    result -> status.setText("Saved; press Reload"));
            } catch (NumberFormatException error) {
                status.setText("Year must be an integer");
            }
        };
        add.setOnAction(event -> write.accept(0L));
        save.setOnAction(event -> write.accept(
            list.getSelectionModel().getSelectedItem().id()));
        delete.setOnAction(event -> {
            long id = list.getSelectionModel().getSelectedItem().id();
            run(() -> { service.delete(id); return id; },
                result -> status.setText("Deleted; press Reload"));
        });
        stage.setOnCloseRequest(event -> {
            if (busy.get()) {
                event.consume();
                status.setText("Wait for operation before closing");
            }
        });
        stage.setScene(new Scene(new VBox(8, list, title, year,
            new HBox(8, load, add, save, delete), status), 620, 430));
        stage.setTitle("Books / PostgreSQL");
        stage.show();
    }

    @Override public void stop() { worker.shutdown(); }
    public static void main(String[] args) { launch(args); }
}
