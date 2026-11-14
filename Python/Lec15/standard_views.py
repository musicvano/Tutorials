import sys
from PySide6.QtCore import Qt
from PySide6.QtGui import QStandardItem, QStandardItemModel
from PySide6.QtWidgets import (
    QApplication, QHBoxLayout, QHeaderView, QLabel, QListView,
    QTableView, QTreeView, QVBoxLayout, QWidget,
)


class ViewsWindow(QWidget):
    def __init__(self) -> None:
        super().__init__()
        self.setWindowTitle("Одна модель, три представлення")
        self.model = QStandardItemModel(0, 2, self)
        self.model.setHorizontalHeaderLabels(["Ім’я", "Оцінка"])
        for name, grade in (("Анна", 85), ("Олег", 40), ("Ірина", 92)):
            grade_item = QStandardItem()
            grade_item.setData(grade, Qt.ItemDataRole.EditRole)
            self.model.appendRow([QStandardItem(name), grade_item])
        self.list = QListView()
        self.table = QTableView()
        self.tree = QTreeView()
        layout = QHBoxLayout(self)
        for title, view in (
            ("QListView", self.list), ("QTableView", self.table),
            ("QTreeView", self.tree),
        ):
            view.setModel(self.model)
            view.setAccessibleName(title)
            column = QVBoxLayout()
            column.addWidget(QLabel(title))
            column.addWidget(view)
            layout.addLayout(column)
        header = self.table.horizontalHeader()
        header.setSectionResizeMode(QHeaderView.ResizeMode.Stretch)


if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = ViewsWindow()
    window.show()
    raise SystemExit(app.exec())
