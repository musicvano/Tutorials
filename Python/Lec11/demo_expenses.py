# demo_expenses.py
from pathlib import Path
from tempfile import TemporaryDirectory
from expenses import Expense, load, save


with TemporaryDirectory() as folder:
    path = Path(folder) / "expenses.json"
    save(path, [Expense("Транспорт", 3000), Expense("Їжа", 7500)])
    items = load(path)
    print(len(items), sum(item.cents for item in items))
