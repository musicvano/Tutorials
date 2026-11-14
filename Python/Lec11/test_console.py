# test_console.py
import pytest


def greet() -> None:
    name = input("Ім’я: ").strip()
    print(f"Вітаю, {name}!")


def test_greet(monkeypatch: pytest.MonkeyPatch,
               capsys: pytest.CaptureFixture[str]) -> None:
    monkeypatch.setattr("builtins.input", lambda _: "Олена")
    greet()
    assert capsys.readouterr().out == "Вітаю, Олена!\n"


def test_approximation() -> None:
    assert 0.1 + 0.2 == pytest.approx(0.3, abs=1e-12)
