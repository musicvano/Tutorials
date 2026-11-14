import sys
from pathlib import Path
from PySide6.QtCore import Slot
from PySide6.QtGui import QAction, QCloseEvent, QKeySequence
from PySide6.QtWidgets import (
    QApplication, QMainWindow, QMessageBox, QPlainTextEdit,
)


class Notepad(QMainWindow):
    def __init__(self, path: Path) -> None:
        super().__init__()
        self.path = path
        self.setWindowTitle("Нотатник")
        self.editor = QPlainTextEdit()
        self.setCentralWidget(self.editor)
        menu = self.menuBar().addMenu("Файл")
        toolbar = self.addToolBar("Дії")
        for title, shortcut, handler in (
            ("Новий", QKeySequence.StandardKey.New, self.new),
            ("Зберегти", QKeySequence.StandardKey.Save, self.save),
            ("Вихід", QKeySequence.StandardKey.Quit, self.close),
        ):
            action = QAction(title, self)
            action.setShortcut(shortcut)
            action.triggered.connect(handler)
            menu.addAction(action)
            toolbar.addAction(action)
        self.editor.cursorPositionChanged.connect(self.position)
        self.resize(550, 360)
        self.position()

    def may_discard(self) -> bool:
        if not self.editor.document().isModified():
            return True
        answer = QMessageBox.question(
            self, "Незбережені зміни", "Відкинути зміни?",
            QMessageBox.StandardButton.Yes
            | QMessageBox.StandardButton.No,
            QMessageBox.StandardButton.No,
        )
        return answer == QMessageBox.StandardButton.Yes

    @Slot()
    def new(self) -> None:
        if self.may_discard():
            self.editor.clear()
            self.editor.document().setModified(False)

    @Slot()
    def save(self) -> None:
        try:
            self.path.write_text(
                self.editor.toPlainText(), encoding="utf-8"
            )
        except OSError as error:
            self.statusBar().showMessage(f"Помилка: {error}")
            return
        self.editor.document().setModified(False)
        self.statusBar().showMessage(f"Збережено: {self.path}")

    @Slot()
    def position(self) -> None:
        cursor = self.editor.textCursor()
        row = cursor.blockNumber() + 1
        column = cursor.positionInBlock() + 1
        message = f"Рядок {row}, стовпець {column}"
        self.statusBar().showMessage(message)

    def closeEvent(self, event: QCloseEvent) -> None:
        event.accept() if self.may_discard() else event.ignore()


if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = Notepad(Path("note.txt"))
    window.show()
    raise SystemExit(app.exec())
