package demo;

import javafx.application.Application;
import javafx.geometry.Insets;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.control.*;
import javafx.scene.layout.VBox;
import javafx.scene.layout.HBox;
import javafx.scene.paint.Color;
import javafx.stage.Stage;

public final class PaintMain extends Application {
    @Override
    public void start(Stage stage) {
        Canvas canvas = new Canvas(560, 300);
        canvas.setId("canvas");
        var pen = canvas.getGraphicsContext2D();
        ColorPicker color = new ColorPicker(Color.BLACK);
        Slider width = new Slider(1, 20, 3);
        width.setShowTickLabels(true);
        Button clear = new Button("Очистити");
        clear.setId("clear");
        canvas.setOnMousePressed(event -> {
            pen.setStroke(color.getValue());
            pen.setLineWidth(width.getValue());
            pen.beginPath();
            pen.moveTo(event.getX(), event.getY());
        });
        canvas.setOnMouseDragged(event -> {
            pen.lineTo(event.getX(), event.getY());
            pen.stroke();
        });
        clear.setOnAction(event -> pen.clearRect(0, 0,
            canvas.getWidth(), canvas.getHeight()));
        VBox root = new VBox(10,
            new HBox(10, color, width, clear), canvas);
        root.setPadding(new Insets(12));
        stage.setTitle("Полотно");
        stage.setScene(new Scene(root));
        stage.show();
    }

    public static void main(String[] args) { launch(args); }
}
