package demo;

import java.sql.Connection;
import java.sql.DatabaseMetaData;

public final class ConnectMain {
    public static void main(String[] args) throws Exception {
        try (Connection connection = Db.open()) {
            DatabaseMetaData metadata = connection.getMetaData();
            System.out.println(metadata.getDatabaseProductName());
            System.out.println(metadata.getDatabaseMajorVersion());
            System.out.println(metadata.getDriverVersion());
        }
    }
}
