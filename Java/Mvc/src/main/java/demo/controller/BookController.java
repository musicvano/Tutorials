package demo.controller;

import demo.model.Book;
import demo.service.BookService;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.function.Consumer;
import javafx.beans.binding.Bindings;
import javafx.beans.property.ReadOnlyObjectWrapper;
import javafx.beans.property.ReadOnlyStringWrapper;
import javafx.beans.property.SimpleBooleanProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.collections.transformation.SortedList;
import javafx.concurrent.Task;
import javafx.fxml.FXML;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.TextField;

public class BookController {
    private final BookService service;
    private final ExecutorService worker =
        Executors.newSingleThreadExecutor(runnable -> {
            Thread thread = new Thread(runnable, "book-dao");
            thread.setDaemon(true);
            return thread;
        });
    private final SimpleBooleanProperty busy =
        new SimpleBooleanProperty(false);
    private final ObservableList<Book> books =
        FXCollections.observableArrayList();

    @FXML private TableView<Book> bookTable;
    @FXML private TableColumn<Book, Long> idColumn;
    @FXML private TableColumn<Book, String> titleColumn;
    @FXML private TableColumn<Book, Integer> yearColumn;
    @FXML private TextField titleField;
    @FXML private TextField yearField;
    @FXML private Button reloadButton;
    @FXML private Button addButton;
    @FXML private Button saveButton;
    @FXML private Button deleteButton;
    @FXML private Label statusLabel;

    public BookController(BookService service) {
        this.service = service;
    }

    @FXML private void initialize() {
        idColumn.setCellValueFactory(cell ->
            new ReadOnlyObjectWrapper<>(cell.getValue().id()));
        titleColumn.setCellValueFactory(cell ->
            new ReadOnlyStringWrapper(cell.getValue().title()));
        yearColumn.setCellValueFactory(cell ->
            new ReadOnlyObjectWrapper<>(cell.getValue().year()));
        var sorted = new SortedList<>(books);
        sorted.comparatorProperty().bind(bookTable.comparatorProperty());
        bookTable.setItems(sorted);
        bookTable.disableProperty().bind(busy);
        titleField.disableProperty().bind(busy);
        yearField.disableProperty().bind(busy);
        reloadButton.disableProperty().bind(busy);
        var invalid = Bindings.createBooleanBinding(
            () -> titleField.getText().isBlank(),
            titleField.textProperty());
        addButton.disableProperty().bind(busy.or(invalid));
        var absent = bookTable.getSelectionModel()
            .selectedItemProperty().isNull();
        saveButton.disableProperty().bind(
            busy.or(invalid).or(absent));
        deleteButton.disableProperty().bind(busy.or(absent));
        bookTable.getSelectionModel().selectedItemProperty()
            .addListener((value, oldBook, book) -> {
                if (book != null) {
                    titleField.setText(book.title());
                    yearField.setText(Integer.toString(book.year()));
                }
            });
    }

    public boolean isBusy() { return busy.get(); }

    public void showStatus(String text) { statusLabel.setText(text); }

    public void close() { worker.shutdown(); }

    <T> void run(Callable<T> action, Consumer<T> success) {
        if (busy.get()) return;
        busy.set(true);
        statusLabel.setText("Busy");
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
            statusLabel.setText("Operation failed; draft kept");
        });
        task.setOnCancelled(event -> {
            busy.set(false);
            statusLabel.setText("Cancelled; reload to verify DB state");
        });
        worker.execute(task);
    }

    @FXML private void reload() {
        run(service::all, rows -> {
            books.setAll(rows);
            statusLabel.setText("Loaded: " + rows.size());
        });
    }

    private void write(long id) {
        try {
            String text = titleField.getText();
            int number = Integer.parseInt(yearField.getText());
            run(() -> {
                service.save(id, text, number);
                return id;
            },
                result -> statusLabel.setText("Saved; press Reload"));
        } catch (NumberFormatException error) {
            statusLabel.setText("Year must be an integer");
        }
    }

    @FXML private void add() {
        if (addButton.isDisabled()) return;
        write(0L);
    }

    @FXML private void save() {
        write(bookTable.getSelectionModel().getSelectedItem().id());
    }

    @FXML private void delete() {
        long id = bookTable.getSelectionModel().getSelectedItem().id();
        run(() -> { service.delete(id); return id; },
            result -> statusLabel.setText("Deleted; press Reload"));
    }
}
