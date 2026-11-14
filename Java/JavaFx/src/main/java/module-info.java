module demo.fx {
    requires javafx.controls;
    requires javafx.fxml;
    exports demo;
    opens demo to javafx.fxml;
}
