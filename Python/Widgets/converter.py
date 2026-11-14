import math
import sys
from PySide6.QtCore import QLocale, QSignalBlocker, Slot
from PySide6.QtGui import QDoubleValidator
from PySide6.QtWidgets import (
    QApplication, QFormLayout, QLabel, QLineEdit, QWidget,
)


class Converter(QWidget):
    def __init__(self) -> None:
        super().__init__()
        self.setWindowTitle("Конвертер температури")
        self.celsius = QLineEdit("0")
        self.fahrenheit = QLineEdit("32")
        self.kelvin = QLabel("273.15 K")
        self.message = QLabel()
        form = QFormLayout(self)
        form.addRow("°C", self.celsius)
        form.addRow("°F", self.fahrenheit)
        form.addRow("Кельвіни", self.kelvin)
        form.addRow(self.message)
        for field in (self.celsius, self.fahrenheit):
            validator = QDoubleValidator(-1e6, 1e6, 2, field)
            validator.setLocale(QLocale.c())
            validator.setNotation(
                QDoubleValidator.Notation.StandardNotation
            )
            field.setValidator(validator)
        self.celsius.textEdited.connect(self.from_celsius)
        self.fahrenheit.textEdited.connect(self.from_fahrenheit)

    def convert(self, source: QLineEdit, to_c: bool) -> None:
        if not source.hasAcceptableInput():
            self.message.setText("Завершіть числовий запис")
            self.kelvin.setText("–")
            return
        value, valid = QLocale.c().toDouble(source.text())
        celsius = value if to_c else (value - 32) * 5 / 9
        if not valid or not math.isfinite(celsius):
            self.message.setText("Потрібне скінченне число")
            return
        if celsius < -273.15 - 1e-9 or celsius > 100000:
            self.message.setText("Допустимо від -273.15 до 100000 °C")
            self.kelvin.setText("–")
            return
        celsius = max(celsius, -273.15)
        target = self.fahrenheit if to_c else self.celsius
        result = celsius * 9 / 5 + 32 if to_c else celsius
        with QSignalBlocker(target):
            target.setText(f"{result:.2f}")
        self.kelvin.setText(f"{celsius + 273.15:.2f} K")
        self.message.clear()

    @Slot(str)
    def from_celsius(self, text: str) -> None:
        self.convert(self.celsius, True)

    @Slot(str)
    def from_fahrenheit(self, text: str) -> None:
        self.convert(self.fahrenheit, False)


if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = Converter()
    window.show()
    raise SystemExit(app.exec())
