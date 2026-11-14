package ua.knu.converter;

public final class Temperature {
    private Temperature() {}

    public static double fahrenheit(double celsius) {
        if (!Double.isFinite(celsius) || celsius < -273.15) {
            throw new IllegalArgumentException("Invalid temperature");
        }
        double value = celsius * 9 / 5 + 32;
        if (!Double.isFinite(value)) {
            throw new IllegalArgumentException("Result overflow");
        }
        return value;
    }
}
