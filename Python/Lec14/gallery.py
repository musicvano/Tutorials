# Widgets gallery for screenshot 14/04: loads gallery.ui (made in Qt Widgets Designer) at run time.
import sys
from pathlib import Path
from PySide6.QtCore import QFile, QIODevice
from PySide6.QtUiTools import QUiLoader
from PySide6.QtWidgets import QApplication, QWidget


def load_form(path: Path) -> QWidget:
    file = QFile(str(path))
    if not file.open(QIODevice.OpenModeFlag.ReadOnly):
        raise OSError(f"Не вдалося відкрити {path}")
    try:
        form = QUiLoader().load(file)
    finally:
        file.close()
    if form is None:
        raise RuntimeError(f"Неправильна форма {path}")
    return form


if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = load_form(Path(__file__).with_name("gallery.ui"))
    window.show()
    raise SystemExit(app.exec())
