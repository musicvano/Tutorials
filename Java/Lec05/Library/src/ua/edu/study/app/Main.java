package ua.edu.study.app;

import ua.edu.study.model.Book;
import ua.edu.study.model.Library;

public class Main {
    public static void main(String[] args) {
        Book[] source = {new Book("Java notes", 2026)};
        Library library = new Library(source);
        source[0] = new Book("Other", 2025);
        Book[] snapshot = library.getBooks();
        snapshot[0] = null;
        System.out.println(library.size());
        System.out.println(library.getBooks()[0]);
    }
}
