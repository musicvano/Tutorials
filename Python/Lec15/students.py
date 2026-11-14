import sys
from dataclasses import dataclass
from PySide6.QtCore import (
    QAbstractTableModel, QModelIndex, QSortFilterProxyModel, Qt,
)
from PySide6.QtGui import QColor
from PySide6.QtWidgets import (
    QApplication, QLabel, QLineEdit, QPushButton, QTableView,
    QVBoxLayout, QWidget,
)


@dataclass
class Student:
    name: str
    grade: int


class StudentModel(QAbstractTableModel):
    def __init__(self, rows: list[Student]) -> None:
        super().__init__()
        self.rows = list(rows)

    def rowCount(self, parent: QModelIndex = QModelIndex()) -> int:
        return 0 if parent.isValid() else len(self.rows)

    def columnCount(self, parent: QModelIndex = QModelIndex()) -> int:
        return 0 if parent.isValid() else 2

    def data(self, index: QModelIndex, role: int = 0) -> object:
        if not index.isValid():
            return None
        student = self.rows[index.row()]
        if role in (Qt.ItemDataRole.DisplayRole,
                    Qt.ItemDataRole.EditRole):
            if index.column() == 0:
                return student.name
            return student.grade
        if role == Qt.ItemDataRole.ToolTipRole:
            return "Оцінка від 0 до 100"
        if (role == Qt.ItemDataRole.ForegroundRole
                and student.grade < 50):
            return QColor("darkred")
        return None

    def headerData(self, section: int, orientation: Qt.Orientation,
                   role: int = 0) -> object:
        if role != Qt.ItemDataRole.DisplayRole:
            return None
        if orientation == Qt.Orientation.Horizontal:
            return ("Ім’я", "Оцінка")[section]
        return section + 1

    def flags(self, index: QModelIndex) -> Qt.ItemFlag:
        flags = super().flags(index)
        if index.isValid():
            flags |= Qt.ItemFlag.ItemIsEditable
        return flags

    def setData(self, index: QModelIndex, value: object,
                role: int = Qt.ItemDataRole.EditRole) -> bool:
        if not index.isValid() or role != Qt.ItemDataRole.EditRole:
            return False
        student = self.rows[index.row()]
        if index.column() == 0:
            name = str(value).strip()
            if not name:
                return False
            student.name = name
        else:
            text = str(value)
            if not text.isascii() or not text.isdecimal():
                return False
            grade = int(text)
            if not 0 <= grade <= 100:
                return False
            student.grade = grade
        self.dataChanged.emit(index, index, [])
        return True

    def add(self, student: Student) -> None:
        if not student.name.strip() or not 0 <= student.grade <= 100:
            raise ValueError("Неправильний студент")
        row = len(self.rows)
        self.beginInsertRows(QModelIndex(), row, row)
        self.rows.append(student)
        self.endInsertRows()

    def remove(self, row: int) -> bool:
        if not 0 <= row < len(self.rows):
            return False
        self.beginRemoveRows(QModelIndex(), row, row)
        del self.rows[row]
        self.endRemoveRows()
        return True


class StudentsWindow(QWidget):
    def __init__(self) -> None:
        super().__init__()
        self.setWindowTitle("Студенти: модель і пошук")
        self.model = StudentModel([Student("Анна", 85),
                                   Student("Олег", 40)])
        self.proxy = QSortFilterProxyModel(self)
        self.proxy.setSourceModel(self.model)
        self.proxy.setFilterKeyColumn(0)
        self.proxy.setFilterCaseSensitivity(
            Qt.CaseSensitivity.CaseInsensitive
        )
        search = QLineEdit()
        search.setPlaceholderText("Частина імені")
        search.textChanged.connect(self.proxy.setFilterFixedString)
        self.view = QTableView()
        self.view.setModel(self.proxy)
        self.view.setSortingEnabled(True)
        self.details = QLabel("Оберіть рядок")
        selection = self.view.selectionModel()
        selection.currentChanged.connect(self.selected)
        add = QPushButton("Додати")
        add.clicked.connect(
            lambda: self.model.add(Student("Новий", 0))
        )
        remove = QPushButton("Видалити вибраного")
        remove.clicked.connect(self.remove_selected)
        layout = QVBoxLayout(self)
        for widget in (search, self.view, self.details, add, remove):
            layout.addWidget(widget)

    def selected(self, current: QModelIndex,
                 previous: QModelIndex) -> None:
        source = self.proxy.mapToSource(current)
        text = "–"
        if source.isValid():
            text = self.model.rows[source.row()].name
        self.details.setText(text)

    def remove_selected(self) -> None:
        source = self.proxy.mapToSource(self.view.currentIndex())
        if source.isValid():
            self.model.remove(source.row())


if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = StudentsWindow()
    window.show()
    raise SystemExit(app.exec())
