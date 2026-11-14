module demo.fx {
    requires javafx.controls;
    requires javafx.fxml;
    requires java.sql;
    requires org.postgresql.jdbc;
    exports demo;
    opens demo to javafx.fxml;
    opens demo.controller to javafx.fxml;
}
