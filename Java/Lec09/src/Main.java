import java.util.Arrays;
import java.util.NoSuchElementException;
import java.util.Objects;

public class Main {
    static final class Stack<E> {
        private E[] elements;
        private int size;

        @SuppressWarnings("unchecked")
        Stack(int capacity) {
            if (capacity < 1) {
                throw new IllegalArgumentException("capacity");
            }
            elements = (E[]) new Object[capacity];
        }

        void push(E value) {
            Objects.requireNonNull(value);
            if (size == elements.length) {
                int capacity = Math.multiplyExact(size, 2);
                elements = Arrays.copyOf(elements, capacity);
            }
            elements[size++] = value;
        }

        E pop() {
            if (size == 0) throw new NoSuchElementException("empty");
            int index = --size;
            E value = elements[index];
            elements[index] = null;
            return value;
        }

        int size() { return size; }
    }

    public static void main(String[] args) {
        var values = new Stack<String>(1);
        values.push("first");
        values.push("second");
        System.out.println(values.pop());
        System.out.println(values.pop());
        System.out.println(values.size());
        try {
            values.pop();
        } catch (NoSuchElementException error) {
            System.out.println(error.getMessage());
        }
    }
}
