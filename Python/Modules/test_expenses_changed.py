# test_expenses.py
from pathlib import Path
import pytest
from expenses import Expense, load, save


@pytest.mark.parametrize("amount", [0, 1, True])
def test_invalid_amount(amount: object) -> None:
    with pytest.raises(ValueError, match="додатним цілим"):
        Expense("Їжа", amount)


def test_round_trip(tmp_path: Path) -> None:
    expected = [Expense("Транспорт", 3000)]
    path = tmp_path / "expenses.json"
    save(path, expected)
    assert load(path) == expected
    assert "Транспорт" in path.read_text(encoding="utf-8")


def test_bad_shape(tmp_path: Path) -> None:
    path = tmp_path / "expenses.json"
    path.write_text('{"category": "Їжа"}', encoding="utf-8")
    with pytest.raises(ValueError, match="масив"):
        load(path)
