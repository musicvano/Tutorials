package demo;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.util.Properties;

public final class Db {
    private Db() {}

    private static String required(String key) {
        String value = System.getenv(key);
        if (value == null || value.isBlank()) {
            throw new IllegalStateException("Missing " + key);
        }
        return value;
    }

    public static Connection open() throws SQLException {
        Properties properties = new Properties();
        properties.setProperty("user", required("DB_USER"));
        properties.setProperty("password", required("DB_PASSWORD"));
        properties.setProperty("connectTimeout", "5");
        properties.setProperty("socketTimeout", "10");
        return DriverManager.getConnection(required("DB_URL"),
            properties);
    }
}
