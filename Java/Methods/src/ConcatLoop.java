public class ConcatLoop {
    public static void main(String[] args) {
        String[] parts = {"Java", " ", "27"};
        String result = "";
        for (String part : parts) {
            result += part;
        }
        System.out.println(result);
    }
}
