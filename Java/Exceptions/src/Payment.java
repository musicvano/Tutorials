class PaymentException extends Exception {
    PaymentException(String message, Throwable cause) {
        super(message, cause);
    }
}

class InsufficientFundsException extends Exception {
    InsufficientFundsException(long available, long requested) {
        super("Є " + available + ", потрібно " + requested);
    }
}

public class Payment {
    static long parseAmount(String text) throws PaymentException {
        try {
            long amount = Long.parseLong(text);
            if (amount <= 0 || amount > 1_000_000) {
                throw new IllegalArgumentException("Сума 1..1000000");
            }
            return amount;
        } catch (IllegalArgumentException e) {
            throw new PaymentException(
                    "Не вдалося прочитати суму", e);
        }
    }

    static long withdraw(long balance, long amount)
            throws InsufficientFundsException {
        if (balance < 0 || amount <= 0) {
            throw new IllegalArgumentException(
                    "Некоректні аргументи");
        }
        if (amount > balance) {
            throw new InsufficientFundsException(balance, amount);
        }
        return balance - amount;
    }

    public static void main(String[] args) {
        long balance = 5_000;
        try {
            balance = withdraw(balance, parseAmount("7000"));
        } catch (PaymentException | InsufficientFundsException e) {
            System.out.println(e.getMessage());
        }
        System.out.println("Баланс: " + balance);
        try {
            parseAmount("abc");
        } catch (PaymentException e) {
            String type = e.getCause().getClass().getSimpleName();
            System.out.println(type);
        }
    }
}
