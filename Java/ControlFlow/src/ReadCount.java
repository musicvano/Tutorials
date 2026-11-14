import java.util.Scanner;

public class ReadCount {
    public static void main(String[] args) {
        Scanner input = new Scanner(System.in, "UTF-8");
        while (true) {
            System.out.print("Кількість 1..100: ");
            if (!input.hasNext()) {
                System.out.println("Кінець вводу");
                return;
            }
            if (!input.hasNextInt()) {
                System.out.println("Не ціле: " + input.next());
                continue;
            }
            int count = input.nextInt();
            if (count < 1 || count > 100) {
                System.out.println("Поза межами");
                continue;
            }
            System.out.println("Прийнято: " + count);
            break;
        }
    }
}
