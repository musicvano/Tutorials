package demo;

import javafx.beans.property.SimpleStringProperty;
import javafx.beans.property.StringProperty;
import javafx.beans.value.ChangeListener;

public class PropertyMain {
    static final class Book {
        private final StringProperty title =
            new SimpleStringProperty(this, "title", "Draft");

        String getTitle() { return title.get(); }
        void setTitle(String value) { title.set(value); }
        StringProperty titleProperty() { return title; }
    }

    public static void main(String[] args) {
        Book book = new Book();
        ChangeListener<String> listener = (value, oldText, newText) ->
            System.out.println(oldText + " -> " + newText);
        book.titleProperty().addListener(listener);
        book.setTitle("Java");
        book.titleProperty().removeListener(listener);
        book.setTitle("Java 27");
        System.out.println(book.getTitle());
    }
}
