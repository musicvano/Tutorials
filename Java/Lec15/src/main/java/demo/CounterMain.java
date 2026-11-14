package demo;

import javafx.application.Application;
import javafx.geometry.Insets;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.layout.HBox;
import javafx.scene.layout.VBox;
import javafx.stage.Stage;

public final class CounterMain extends Application {
    private int count;

    @Override
    public void start(Stage stage) {
        Label value = new Label("0");
        value.setId("value");
        Button add = new Button("Додати");
        add.setId("add");
        Button reset = new Button("Скинути");
        reset.setId("reset");
        add.setOnAction(event -> {
            count = Math.incrementExact(count);
            value.setText(Integer.toString(count));
        });
        reset.setOnAction(event -> {
            count = 0;
            value.setText("0");
        });
        VBox root = new VBox(12, value, new HBox(8, add, reset));
        root.setPadding(new Insets(16));
        stage.setTitle("Лічильник");
        stage.setScene(new Scene(root, 320, 160));
        stage.show();
    }

    public static void main(String[] args) { launch(args); }
}
