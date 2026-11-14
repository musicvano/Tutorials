# expenses.py
import json
from dataclasses import asdict, dataclass
from pathlib import Path


@dataclass(frozen=True)
class Expense:
    category: str
    cents: int

    def __post_init__(self) -> None:
        if not isinstance(self.category, str):
            raise ValueError("категорія має бути рядком")
        if not self.category.strip():
            raise ValueError("категорія порожня")
        if type(self.cents) is not int or self.cents <= 0:
            raise ValueError("сума має бути додатним цілим")


def save(path: Path, items: list[Expense]) -> None:
    text = json.dumps([asdict(item) for item in items],
                      ensure_ascii=False, indent=2,
                      allow_nan=False)
    path.write_text(text, encoding="utf-8")


def load(path: Path) -> list[Expense]:
    data = json.loads(path.read_text(encoding="utf-8"))
    if not isinstance(data, list):
        raise ValueError("потрібен масив записів")
    result: list[Expense] = []
    for row in data:
        if not isinstance(row, dict):
            raise ValueError("запис має бути об’єктом")
        if set(row) != {"category", "cents"}:
            raise ValueError("неправильні поля запису")
        result.append(Expense(row["category"], row["cents"]))
    return result
