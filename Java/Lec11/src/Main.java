import java.util.ArrayList;
import java.util.List;
import java.util.function.Predicate;

public class Main {
    record Employee(String name, int score, boolean active) {}

    @FunctionalInterface
    interface Rule<T> {
        boolean accepts(T value);
    }

    static <T> List<T> select(List<T> source, Rule<? super T> rule) {
        List<T> result = new ArrayList<>();
        for (T item : source) {
            if (rule.accepts(item)) result.add(item);
        }
        return result;
    }

    public static void main(String[] args) {
        List<Employee> staff = List.of(
            new Employee("Ada", 90, true),
            new Employee("Bohdan", 95, false),
            new Employee("Ira", 70, true)
        );
        int minimum = 80;
        Predicate<Employee> active = Employee::active;
        Predicate<Employee> enough = e -> e.score() >= minimum;
        Predicate<Employee> combined = active.and(enough);
        List<Employee> result = select(staff, combined::test);
        result.forEach(e -> System.out.println(e.name()));
        System.out.println("source size: " + staff.size());
    }
}
