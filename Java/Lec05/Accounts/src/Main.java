class BankAccount {
    private static int created;
    private static final long LIMIT = 1_000_000_000L;
    private final String number;
    private long balance;

    BankAccount(String number, long initial) {
        if (number == null || number.isBlank()) {
            throw new IllegalArgumentException("Empty number");
        }
        if (initial < 0 || initial > LIMIT) {
            throw new IllegalArgumentException("Invalid balance");
        }
        this.number = number;
        this.balance = initial;
        created++;
    }

    BankAccount(String number) {
        this(number, 0);
    }

    public long getBalance() { return balance; }
    public static int getCreated() { return created; }

    public void deposit(long amount) {
        if (amount <= 0 || amount > LIMIT - balance) {
            throw new IllegalArgumentException("Invalid deposit");
        }
        balance += amount;
    }

    public void withdraw(long amount) {
        if (amount <= 0 || amount > balance) {
            throw new IllegalArgumentException("Invalid withdrawal");
        }
        balance -= amount;
    }

    public String toString() { return number + ": " + balance; }
}

public class Main {
    public static void main(String[] args) {
        BankAccount a = new BankAccount("A1", 1000);
        BankAccount b = a;
        BankAccount c = new BankAccount("A2");
        b.deposit(500);
        a.withdraw(200);
        System.out.println(a);
        System.out.println(c);
        System.out.println(a == b);
        try {
            a.withdraw(2000);
        } catch (IllegalArgumentException error) {
            System.out.println(error.getMessage());
        }
        System.out.println(a.getBalance());
        System.out.println(BankAccount.getCreated());
    }
}
