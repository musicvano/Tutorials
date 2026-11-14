package demo.model;

public record Book(long id, String title, int year) {
    @Override public String toString() {
        return id + ": " + title + " (" + year + ")";
    }
}
