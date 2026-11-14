from abc import ABC, abstractmethod
from math import isfinite, pi, sqrt
from typing import override


class Shape(ABC):
    @abstractmethod
    def area(self) -> float:
        raise NotImplementedError

    @abstractmethod
    def perimeter(self) -> float:
        raise NotImplementedError

    def describe(self) -> str:
        return f"{type(self).__name__}: {self.area():.2f}"


def positive(*values: float) -> None:
    if not all(isfinite(v) and v > 0 for v in values):
        raise ValueError("Розміри мають бути додатними")


class Circle(Shape):
    def __init__(self, radius: float) -> None:
        positive(radius)
        self._radius = radius

    @override
    def area(self) -> float:
        return pi * self._radius ** 2

    @override
    def perimeter(self) -> float:
        return 2 * pi * self._radius


class Rectangle(Shape):
    def __init__(self, width: float, height: float) -> None:
        positive(width, height)
        self._width, self._height = width, height

    @override
    def area(self) -> float:
        return self._width * self._height

    @override
    def perimeter(self) -> float:
        return 2 * (self._width + self._height)


class Square(Rectangle):
    def __init__(self, side: float) -> None:
        super().__init__(side, side)


class Triangle(Shape):
    def __init__(self, a: float, b: float, c: float) -> None:
        positive(a, b, c)
        if 2 * max(a, b, c) >= a + b + c:
            raise ValueError("Порушено нерівність трикутника")
        self._a, self._b, self._c = a, b, c


shapes: list[Shape] = [Circle(1), Rectangle(2, 3),
                       Square(2), Triangle(3, 4, 5)]
for shape in sorted(shapes, key=lambda item: item.area()):
    print(shape.describe())
