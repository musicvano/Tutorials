package ua.edu.study.model;

public final class Book {
    private final String title;
    private final int year;

    public Book(String title, int year) {
        if (title == null || title.isBlank()
                || year < 1450 || year > 2100) {
            throw new IllegalArgumentException("Invalid book");
        }
        this.title = title;
        this.year = year;
    }

    public String getTitle() { return title; }
    public String toString() { return title + " (" + year + ")"; }
}
