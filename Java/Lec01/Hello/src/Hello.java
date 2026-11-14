public class Hello {
    public static void main(String[] args) {
        String name = args.length == 0 ? "студент" : args[0];
        System.out.println("Вітаю, " + name + "!");
        System.out.println("Аргументів: " + args.length);
    }
}
