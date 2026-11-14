public class AssertionDemo {
    public static void main(String[] args) {
        int[] values = {1, 2, 3};
        assert values.length > 0 : "Порожній масив";
        int sum = 0;
        for (int value : values) sum += value;
        assert sum == 6 : "Порушений контрольний приклад";
        System.out.println(sum);
    }
}
