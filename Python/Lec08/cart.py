class Product:
    def __init__(self, name: str, price: int) -> None:
        if not name.strip() or type(price) is not int or price < 0:
            raise ValueError("Некоректна позиція")
        self.name = name.strip()
        self.price = price

    def __repr__(self) -> str:
        return f"Product({self.name!r}, {self.price})"


class Cart:
    def __init__(self, discount: int = 0) -> None:
        if not 0 <= discount <= 100:
            raise ValueError("Знижка поза 0..100")
        self.__discount = discount
        self._items: list[Product] = []

    def add(self, name: str, price: int) -> None:
        self._items.append(Product(name, price))

    @property
    def total(self) -> int:
        subtotal = sum(item.price for item in self._items)
        return subtotal * (100 - self.__discount) // 100

    def __repr__(self) -> str:
        return f"Cart(items={self._items!r}, total={self.total})"


cart = Cart(10)
cart.add("чай", 4000)
cart.add("хліб", 6000)
print(cart.total)
print(len(cart._items))  # лише демонстрація налагодження
