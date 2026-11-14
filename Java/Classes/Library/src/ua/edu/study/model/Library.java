package ua.edu.study.model;

public final class Library {
    private final Book[] books;

    public Library(Book[] books) {
        this.books = java.util.Objects.requireNonNull(books).clone();
        for (Book book : this.books) {
            java.util.Objects.requireNonNull(book);
        }
    }

    public Book[] getBooks() { return books.clone(); }
    public int size() { return books.length; }
}
