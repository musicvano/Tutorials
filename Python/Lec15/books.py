import sys
from PySide6.QtCore import QSettings
from PySide6.QtGui import QCloseEvent
from PySide6.QtSql import (
    QSqlDatabase, QSqlQuery, QSqlRelation, QSqlRelationalDelegate,
    QSqlRelationalTableModel, QSqlTableModel,
)
from PySide6.QtWidgets import (
    QApplication, QHBoxLayout, QLabel, QPushButton,
    QTableView, QVBoxLayout, QWidget,
)


def open_database(path: str, name: str) -> QSqlDatabase:
    db = QSqlDatabase.addDatabase("QSQLITE", name)
    db.setDatabaseName(path)
    if not db.open():
        raise RuntimeError(db.lastError().text())
    query = QSqlQuery(db)
    commands = [
        "PRAGMA foreign_keys=ON",
        "CREATE TABLE IF NOT EXISTS authors "
        "(id INTEGER PRIMARY KEY, name TEXT NOT NULL)",
        "CREATE TABLE IF NOT EXISTS books "
        "(id INTEGER PRIMARY KEY, title TEXT NOT NULL "
        "CHECK(length(trim(title))>0), author_id INTEGER "
        "NOT NULL REFERENCES authors(id))",
        "INSERT OR IGNORE INTO authors VALUES (1, 'Леся Українка')",
        "INSERT OR IGNORE INTO authors VALUES (2, 'Іван Франко')",
    ]
    for sql in commands:
        if not query.exec(sql):
            raise RuntimeError(query.lastError().text())
    return db


class BooksWindow(QWidget):
    def __init__(self, db: QSqlDatabase) -> None:
        super().__init__()
        self.db = db
        self.setWindowTitle("Каталог книг")
        self.settings = QSettings("CourseDemo", "Books15")
        geometry = self.settings.value("geometry")
        if geometry is not None:
            self.restoreGeometry(geometry)
        self.model = QSqlRelationalTableModel(self, db)
        self.model.setTable("books")
        relation = QSqlRelation("authors", "id", "name")
        self.model.setRelation(2, relation)
        strategy = QSqlTableModel.EditStrategy.OnManualSubmit
        self.model.setEditStrategy(strategy)
        if not self.model.select():
            raise RuntimeError(self.model.lastError().text())
        self.view = QTableView()
        self.view.setModel(self.model)
        self.view.setItemDelegate(QSqlRelationalDelegate(self.view))
        self.view.hideColumn(0)
        self.status = QLabel("Зміни ще не збережено")
        buttons = QHBoxLayout()
        for title, handler in (
            ("Додати", self.add), ("Видалити", self.remove),
            ("Зберегти", self.save), ("Скасувати", self.revert),
        ):
            button = QPushButton(title)
            button.clicked.connect(handler)
            buttons.addWidget(button)
        layout = QVBoxLayout(self)
        layout.addWidget(self.view)
        layout.addLayout(buttons)
        layout.addWidget(self.status)

    def add(self) -> None:
        record = self.model.record()
        record.setNull(0)
        record.setValue(1, "Нова книга")
        record.setValue(2, 1)
        if not self.model.insertRecord(-1, record):
            self.status.setText(self.model.lastError().text())

    def remove(self) -> None:
        index = self.view.currentIndex()
        if index.isValid():
            self.model.removeRow(index.row())

    def save(self) -> bool:
        if not self.db.transaction():
            self.status.setText(self.db.lastError().text())
            return False
        if not self.model.submitAll():
            message = self.model.lastError().text()
            self.db.rollback()
            self.status.setText(message)
            return False
        if not self.db.commit():
            message = self.db.lastError().text()
            self.db.rollback()
            self.model.select()
            self.status.setText(message)
            return False
        self.status.setText("Збережено")
        return True

    def revert(self) -> None:
        self.model.revertAll()
        self.model.select()
        self.status.setText("Незбережені зміни скасовано")

    def closeEvent(self, event: QCloseEvent) -> None:
        if self.model.isDirty():
            self.status.setText("Збережіть або скасуйте зміни")
            event.ignore()
            return
        self.settings.setValue("geometry", self.saveGeometry())
        event.accept()


if __name__ == "__main__":
    app = QApplication(sys.argv)
    database = open_database("books.sqlite", "books-demo")
    window = BooksWindow(database)
    window.show()
    code = app.exec()
    database.close()
    raise SystemExit(code)
