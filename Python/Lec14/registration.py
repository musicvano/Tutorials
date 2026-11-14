import sys
from PySide6.QtCore import Signal, Slot
from PySide6.QtWidgets import (
    QApplication, QCheckBox, QFormLayout, QLabel, QLineEdit,
    QPushButton, QSpinBox, QWidget,
)


class Registration(QWidget):
    registered = Signal(str, int)

    def __init__(self) -> None:
        super().__init__()
        self.setWindowTitle("Форма реєстрації")
        self.name = QLineEdit()
        self.name.setPlaceholderText("Ім’я учасника")
        self.age = QSpinBox()
        self.age.setRange(16, 100)
        self.consent = QCheckBox("Погоджуюся з правилами")
        self.submit = QPushButton("Зареєструвати")
        self.result = QLabel()
        layout = QFormLayout(self)
        layout.addRow("Ім’я", self.name)
        layout.addRow("Вік", self.age)
        for widget in (self.consent, self.submit, self.result):
            layout.addRow(widget)
        self.name.textChanged.connect(self.validate)
        self.consent.toggled.connect(self.validate)
        self.submit.clicked.connect(self.send)
        self.validate()

    @Slot()
    def validate(self) -> None:
        valid = bool(self.name.text().strip())
        self.submit.setEnabled(valid and self.consent.isChecked())

    @Slot()
    def send(self) -> None:
        self.validate()
        if not self.submit.isEnabled():
            return
        name = self.name.text().strip()
        self.registered.emit(name, self.age.value())
        self.result.setText(f"Зареєстровано: {name}")


if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = Registration()
    window.show()
    raise SystemExit(app.exec())
