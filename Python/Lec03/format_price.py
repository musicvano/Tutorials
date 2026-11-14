def format_price(amount: float, currency: str = "UAH",
                 *, precision: int = 2) -> str:
    """Форматувати суму; precision від 0 до 4."""
    return f"{amount:.{precision}f} {currency}"


print(format_price(12.5))
print(format_price(currency="EUR", amount=7, precision=1))
