from dataclasses import dataclass, field, replace
from enum import StrEnum, auto


class Status(StrEnum):
    NEW = auto()
    PAID = auto()


@dataclass(frozen=True, slots=True)
class Product:
    name: str
    cents: int

    def __post_init__(self) -> None:
        if not self.name.strip() or self.cents < 0:
            raise ValueError("неправильний товар")


@dataclass
class Order:
    products: list[Product] = field(default_factory=list)
    status: Status = Status.NEW

    def total(self) -> int:
        return sum(p.cents for p in self.products)

    def pay(self) -> None:
        if not self.products or self.status is not Status.NEW:
            raise ValueError("оплата недоступна")
        self.status = Status.PAID


book = Product("Книга", 12000)
first = Order([book, replace(book, cents=10000)])
second = Order()
first.pay()
print(first.total(), first.status.value)
print(len(second.products))
match first:
    case Order(status=Status.PAID):
        print("Можна передавати на доставку")
    case _:
        print("Очікуємо оплату")
