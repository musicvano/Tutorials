class BankAccount:
    bank_name = "Навчальний банк"

    def __init__(self, owner: str, pin: str) -> None:
        if not owner.strip() or not (len(pin) == 4
                and pin.isascii() and pin.isdecimal()):
            raise ValueError("Потрібні власник і PIN із 4 цифр")
        self.owner = owner.strip()
        self.__pin = pin
        self._balance = 0

    @property
    def balance(self) -> int:
        return self._balance

    def deposit(self, amount: int) -> None:
        if type(amount) is not int or amount <= 0:
            raise ValueError("Сума має бути додатною цілою")
        self._balance += amount

    def withdraw(self, amount: int) -> None:
        if type(amount) is not int or not 0 < amount <= self.balance:
            raise ValueError("Недопустима сума списання")
        self._balance -= amount

    def __repr__(self) -> str:
        return f"BankAccount({self.owner!r}, balance={self.balance})"


account = BankAccount("Олена", "1234")
account.deposit(1000)
account.withdraw(300)
print(account)
try:
    account.withdraw(800)
except ValueError:
    print("Відмовлено; баланс:", account.balance)
