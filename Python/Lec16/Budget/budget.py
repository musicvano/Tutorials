import json
import sys
from pathlib import Path

from PySide6.QtWidgets import (
    QApplication, QLabel, QPushButton, QVBoxLayout, QWidget,
)


def initial_cents() -> int:
    path = Path(__file__).with_name("defaults.json")
    data = json.loads(path.read_text(encoding="utf-8"))
    if not isinstance(data, dict):
        raise ValueError("Потрібен об’єкт")
    value = data.get("cents")
    if type(value) is not int or not 0 <= value <= 1000000:
        raise ValueError("Некоректний баланс")
    return value


class Budget(QWidget):
    def __init__(self) -> None:
        super().__init__()
        self.cents = initial_cents()
        self.setWindowTitle("Навчальний бюджет")
        self.label = QLabel()
        self.button = QPushButton("Витратити 100 грн")
        self.button.clicked.connect(self.spend)
        layout = QVBoxLayout(self)
        layout.addWidget(self.label)
        layout.addWidget(self.button)
        self.refresh()

    def refresh(self) -> None:
        self.label.setText(f"Залишок: {self.cents / 100:.2f}")
        self.button.setEnabled(self.cents >= 10000)

    def spend(self) -> None:
        if self.cents >= 10000:
            self.cents -= 10000
        self.refresh()


def main() -> int:
    app = QApplication(sys.argv)
    window = Budget()
    if "--smoke" in sys.argv:
        before = window.cents
        window.spend()
        expected = before - 10000 if before >= 10000 else before
        return 0 if window.cents == expected else 1
    window.show()
    return app.exec()


if __name__ == "__main__":
    raise SystemExit(main())
