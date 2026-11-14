"""Таблиця квадратів чисел."""
from rich import box
from rich.console import Console
from rich.table import Table

table = Table(box=box.ASCII)
table.add_column("n", justify="right")
table.add_column("n^2", justify="right")
for number in (2, 3, 4):
    table.add_row(str(number), str(number ** 2))
Console(color_system=None).print(table)
