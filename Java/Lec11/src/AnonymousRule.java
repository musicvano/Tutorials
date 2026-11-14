public class AnonymousRule {
    record Employee(String name, int score, boolean active) {}

    @FunctionalInterface
    interface Rule<T> {
        boolean accepts(T value);
    }

    public static void main(String[] args) {
        int minimum = 80;
        Rule<Employee> enough = new Rule<>() {
            @Override
            public boolean accepts(Employee e) {
                return e.score() >= minimum;
            }
        };
        Employee ada = new Employee("Ada", 90, true);
        System.out.println(ada.name() + ": " + enough.accepts(ada));
    }
}
