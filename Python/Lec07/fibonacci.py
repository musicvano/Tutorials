from collections.abc import Iterator
from itertools import islice, takewhile


def fibonacci() -> Iterator[int]:
    a, b = 0, 1
    while True:
        yield a
        a, b = b, a + b


print(list(islice(fibonacci(), 8)))
print(list(takewhile(lambda x: x < 20, fibonacci())))
