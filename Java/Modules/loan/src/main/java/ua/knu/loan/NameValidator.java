package ua.knu.loan;

public final class NameValidator {
    private NameValidator() {}
    public static String normalize(String value) {
        if (value == null) {
            throw new IllegalArgumentException("Missing name");
        }
        String text = value.trim();
        if (text.length() < 2 || text.length() > 40) {
            throw new IllegalArgumentException("Length 2..40");
        }
        return text;
    }
}
