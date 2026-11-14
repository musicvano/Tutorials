package ua.edu.study.util;

public final class Temperatures {
    private Temperatures() { }

    /**
     * Converts a finite Celsius temperature to Fahrenheit.
     * @param celsius temperature, not below absolute zero
     * @return temperature in Fahrenheit
     * @throws IllegalArgumentException for invalid input
     */
    public static double toFahrenheit(double celsius) {
        if (!Double.isFinite(celsius) || celsius < -273.15
                || celsius > 1_000_000) {
            throw new IllegalArgumentException("Invalid temperature");
        }
        return celsius * 9 / 5 + 32;
    }
}
