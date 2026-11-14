public class PositiveAverage {
    static Double average(int[] values) {
        long sum = 0;
        int count = 0;
        for (int value : values) {
            if (value > 0) {
                sum += value;
                count++;
            }
        }
        if (count == 0) return null;
        return (double) sum / count;
    }

    public static void main(String[] args) {
        System.out.println(average(new int[]{10, -5, 20}));
        System.out.println(average(new int[]{-5, 0}));
    }
}
