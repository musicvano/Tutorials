class Employee {
    private final String name;
    private final long base;

    Employee(String name, long base) {
        if (name == null || name.isBlank()
                || base < 0 || base > 100_000_000) {
            throw new IllegalArgumentException("Invalid employee");
        }
        this.name = name.strip();
        this.base = base;
    }

    public long salary() { return base; }
    public final String getName() { return name; }

    @Override
    public String toString() {
        return name + ": " + salary();
    }
}

final class HourlyEmployee extends Employee {
    private final int hours;
    private final long rate;

    HourlyEmployee(String name, long base, int hours, long rate) {
        if (hours < 0 || hours > 250 || rate < 0
                || rate > 1_000_000) {
            throw new IllegalArgumentException("Invalid hours/rate");
        }
        super(name, base);
        this.hours = hours;
        this.rate = rate;
    }

    @Override
    public long salary() {
        return super.salary() + hours * rate;
    }
}

public class Main {
    public static void main(String[] args) {
        Employee[] staff = {
            new Employee("Olena", 200_000),
            new HourlyEmployee("Taras", 100_000, 20, 5_000)
        };
        long total = 0;
        for (Employee employee : staff) {
            System.out.println(employee);
            total += employee.salary();
        }
        System.out.println("Total: " + total);
        try {
            new HourlyEmployee("Ivan", 0, -1, 5_000);
        } catch (IllegalArgumentException ex) {
            System.out.println(ex.getMessage());
        }
    }
}
