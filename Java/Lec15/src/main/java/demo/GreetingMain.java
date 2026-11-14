package demo;

import java.util.Objects;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.stage.Stage;

public final class GreetingMain extends Application {
    @Override
    public void start(Stage stage) throws Exception {
        var fxml = Objects.requireNonNull(
            getClass().getResource("/demo/greeting.fxml"),
            "Missing greeting.fxml");
        var css = Objects.requireNonNull(
            getClass().getResource("/demo/app.css"),
            "Missing app.css");
        Scene scene = new Scene(FXMLLoader.load(fxml), 400, 210);
        scene.getStylesheets().add(css.toExternalForm());
        stage.setScene(scene);
        stage.setTitle("Привітання");
        stage.show();
    }

    public static void main(String[] args) { launch(args); }
}
