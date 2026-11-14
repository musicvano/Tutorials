package demo;

import javafx.fxml.FXML;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;

public final class GreetingController {
    @FXML private TextField name;
    @FXML private Label status;

    @FXML
    private void initialize() {
        status.setText("Введіть ім’я та натисніть кнопку");
    }

    @FXML
    private void greet() {
        String value = name.getText().strip();
        if (value.isEmpty() || value.length() > 40) {
            status.setText("Потрібно від 1 до 40 символів");
            name.requestFocus();
            return;
        }
        status.setText("Вітаємо, " + value + "!");
    }
}
