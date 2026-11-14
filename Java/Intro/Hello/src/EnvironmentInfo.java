public class EnvironmentInfo {
    public static void main(String[] args) {
        System.out.println("Java: " + Runtime.version());
        System.out.println("Vendor: "
                + System.getProperty("java.vendor"));
        System.out.println("OS: "
                + System.getProperty("os.name"));
        System.out.println("Working directory: "
                + System.getProperty("user.dir"));
    }
}
