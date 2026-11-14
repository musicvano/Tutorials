package demo;

import demo.controller.BookController;
import demo.dao.BookRepository;
import demo.dao.JdbcBookRepository;
import demo.service.BookService;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;

public class BookApp extends Application {
    private BookController controller;

    protected BookRepository repository() {
        return new JdbcBookRepository();
    }

    @Override public void start(Stage stage) throws Exception {
        BookService service = new BookService(repository());
        FXMLLoader loader = new FXMLLoader(
            BookApp.class.getResource("/view/books.fxml"));
        loader.setControllerFactory(type -> new BookController(service));
        Parent root = loader.load();
        controller = loader.getController();
        stage.setOnCloseRequest(event -> {
            if (controller.isBusy()) {
                event.consume();
                controller.showStatus("Wait for operation before closing");
            }
        });
        stage.setScene(new Scene(root, 620, 430));
        stage.setTitle("Books / FXML");
        stage.show();
    }

    @Override public void stop() {
        if (controller != null) controller.close();
    }

    public static void main(String[] args) { launch(args); }
}
