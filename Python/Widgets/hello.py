import sys
from PySide6.QtWidgets import (
    QApplication, QLabel, QPushButton, QVBoxLayout, QWidget,
)

if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = QWidget()
    window.setWindowTitle("Привіт, Qt")
    layout = QVBoxLayout(window)
    layout.addWidget(QLabel("Мій перший застосунок"))
    close = QPushButton("Закрити")
    close.clicked.connect(window.close)
    layout.addWidget(close)
    window.show()
    raise SystemExit(app.exec())
