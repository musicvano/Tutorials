package ua.edu.study.app;

import static ua.edu.study.util.Temperatures.toFahrenheit;

public class Main {
    public static void main(String[] args) {
        System.out.println(toFahrenheit(0));
        System.out.println(toFahrenheit(25));
        try {
            toFahrenheit(Double.NaN);
        } catch (IllegalArgumentException error) {
            System.out.println(error.getMessage());
        }
    }
}
