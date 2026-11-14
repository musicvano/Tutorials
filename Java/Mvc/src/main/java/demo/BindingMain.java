package demo;

import javafx.application.Application;
import javafx.beans.binding.Bindings;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.control.Spinner;
import javafx.scene.layout.VBox;
import javafx.stage.Stage;

public class BindingMain extends Application {
    @Override public void start(Stage stage) {
        Spinner<Integer> quantity = new Spinner<>(0, 100, 2);
        Label total = new Label();
        total.textProperty().bind(Bindings.createStringBinding(
            () -> "Total cents: " + quantity.getValue() * 250L,
            quantity.valueProperty()));
        Button save = new Button("Save");
        save.disableProperty().bind(Bindings.createBooleanBinding(
            () -> quantity.getValue() == 0,
            quantity.valueProperty()));
        save.setOnAction(event ->
            System.out.println(total.getText()));
        stage.setScene(new Scene(new VBox(10, quantity, total, save),
            340, 180));
        stage.setTitle("Order binding");
        stage.show();
    }

    public static void main(String[] args) { launch(args); }
}
