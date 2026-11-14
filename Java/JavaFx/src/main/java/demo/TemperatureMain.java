package demo;

import java.util.Locale;
import javafx.application.Application;
import javafx.geometry.Insets;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.GridPane;
import javafx.stage.Stage;

public final class TemperatureMain extends Application {
    public static double fahrenheit(String input) {
        double c = Double.parseDouble(
            input.strip().replace(',', '.'));
        if (!Double.isFinite(c) || c < -273.15 || c > 1_000_000) {
            throw new IllegalArgumentException("Поза межами");
        }
        return c * 1.8 + 32;
    }

    @Override
    public void start(Stage stage) {
        TextField input = new TextField();
        input.setId("input");
        input.setPromptText("Наприклад, 20,5");
        Label output = new Label("Введіть температуру");
        output.setId("output");
        Label caption = new Label("Градуси Цельсія:");
        caption.setLabelFor(input);
        Button convert = new Button("Обчислити");
        convert.setId("convert");
        convert.setDefaultButton(true);
        convert.setOnAction(event -> {
            try {
                double result = fahrenheit(input.getText());
                output.setText(String.format(Locale.ROOT,
                    "%.2f °F", result));
            } catch (IllegalArgumentException error) {
                output.setText(
                    "Потрібне число від -273,15 до 1000000");
                input.requestFocus();
            }
        });
        GridPane root = new GridPane();
        root.setHgap(10);
        root.setVgap(12);
        root.setPadding(new Insets(16));
        root.addRow(0, caption, input);
        root.addRow(1, convert, output);
        stage.setTitle("Температура");
        stage.setScene(new Scene(root, 540, 150));
        stage.show();
    }

    public static void main(String[] args) { launch(args); }
}
