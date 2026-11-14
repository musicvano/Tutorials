package demo;

import java.util.Locale;
import javafx.application.Application;
import javafx.beans.property.ReadOnlyStringWrapper;
import javafx.collections.FXCollections;
import javafx.collections.transformation.FilteredList;
import javafx.collections.transformation.SortedList;
import javafx.scene.Scene;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.TextField;
import javafx.scene.layout.VBox;
import javafx.stage.Stage;

public class TableMain extends Application {
    record Book(String title) {}

    @Override public void start(Stage stage) {
        var source = FXCollections.observableArrayList(
            new Book("Java"), new Book("SQL"), new Book("JavaFX"));
        var filtered = new FilteredList<>(source, book -> true);
        var sorted = new SortedList<>(filtered);
        TableView<Book> table = new TableView<>();
        TableColumn<Book, String> title = new TableColumn<>("Title");
        title.setCellValueFactory(cell ->
            new ReadOnlyStringWrapper(cell.getValue().title()));
        table.getColumns().add(title);
        sorted.comparatorProperty().bind(table.comparatorProperty());
        table.setItems(sorted);
        TextField search = new TextField();
        search.setPromptText("Search title");
        search.textProperty().addListener((value, oldText, text) -> {
            String query = text.strip().toLowerCase(Locale.ROOT);
            filtered.setPredicate(book -> book.title()
                .toLowerCase(Locale.ROOT).contains(query));
        });
        stage.setScene(new Scene(
            new VBox(10, search, table), 400, 300));
        stage.setTitle("Book search");
        stage.show();
    }

    public static void main(String[] args) { launch(args); }
}
