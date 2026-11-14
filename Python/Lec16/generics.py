from collections.abc import Sequence


type Pair[T] = tuple[T, T]


def first[T](items: Sequence[T]) -> T:
    if not items:
        raise ValueError("Порожня послідовність")
    return items[0]


class Stack[T = str]:
    def __init__(self) -> None:
        self._items: list[T] = []

    def push(self, item: T) -> None:
        self._items.append(item)

    def pop(self) -> T:
        if not self._items:
            raise IndexError("Порожній стек")
        return self._items.pop()


def main() -> None:
    numbers = Stack[int]()
    numbers.push(first([10, 20]))
    numbers.push(30)
    print(numbers.pop(), numbers.pop())
    pair: Pair[str] = ("код", "тест")
    print(first(pair))


if __name__ == "__main__":
    main()
