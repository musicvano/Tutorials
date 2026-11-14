# Object-oriented programming in Python

Code examples for the course [Object-oriented programming in Python](https://mvano.com/courses/python/).
Each `LecNN` folder belongs to topic NN of the course.

## Tools

Python 3.14 and PyCharm 2026.2 (or VS Code). Open this `Python` folder as the project and create one
virtual environment for all topics:

```powershell
py -3.14 -m venv .venv
.venv\Scripts\python -m pip install rich pytest sqlalchemy numpy pandas matplotlib PySide6 pyinstaller
.venv\Scripts\python Lec02\quadratic.py
```

| Package                       | Topics |
| ----------------------------- | ------ |
| `rich`                        | 1      |
| `pytest`                      | 11     |
| `sqlalchemy`                  | 12     |
| `numpy`, `pandas`, `matplotlib` | 13   |
| `PySide6`                     | 14, 15 |
| `pyinstaller`                 | 16     |

## Topics

| Folder            | Topic                                                                       | Contents                                                                                       |
| ----------------- | --------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------- |
| [`Lec01`](Lec01)  | Python 3.14, PyCharm, virtual environments, packages, Git                   | `hello\` (first project with `requirements.txt`), `args.py`, `environment.py`, `broken\`      |
| [`Lec02`](Lec02)  | Data types, variables, operations, console input/output, control flow       | `quadratic.py`, `pep8_demo.py`, `pep8_names.py`                                                |
| [`Lec03`](Lec03)  | Functions: parameters, scope, recursion, type hints                         | `area.py`, `area_type_warning.py`, `format_price.py`, `factorial_broken.py`                    |
| [`Lec04`](Lec04)  | Exception handling and debugging                                            | `average.py`, `average_single.py`, `traceback_demo.py`                                         |
| [`Lec05`](Lec05)  | Built-in collections and the `collections` module                           | `journal.py` (group grade journal)                                                             |
| [`Lec06`](Lec06)  | Strings, f-strings, template strings, regular expressions                    | `phone.py`, `tstring.py`, `dates.txt`                                                          |
| [`Lec07`](Lec07)  | Iterators, generators, lambdas, decorators                                  | `fibonacci.py`, `retry_timer.py`                                                               |
| [`Lec08`](Lec08)  | Classes and objects: attributes, methods, properties, encapsulation         | `bank_account.py`, `cart.py`                                                                   |
| [`Lec09`](Lec09)  | Inheritance, polymorphism, abstract classes, protocols                      | `shapes.py`, `shapes_implement.py`                                                             |
| [`Lec10`](Lec10)  | Special methods, operator overloading, data classes, enums                  | `products.py`, `products_frozen.py`                                                            |
| [`Lec11`](Lec11)  | Modules and packages, files (pathlib, JSON, CSV), testing with pytest       | `geometry\` package, `expenses.py` + tests, `grades.py`                                        |
| [`Lec12`](Lec12)  | Relational databases: SQL, `sqlite3`, SQLAlchemy ORM                        | `contacts.py`, `grades.py`, `authors_orm.py`, `school.sql`, `left_join.sql`                    |
| [`Lec13`](Lec13)  | NumPy, pandas, and Matplotlib                                               | `sales.py`, `sensor_report.py`                                                                 |
| [`Lec14`](Lec14)  | PySide6 GUI applications: widgets, layouts, signals and slots               | `hello.py`, `converter.py`, `registration.py`, `notepad.py`, `gallery.py`, `.ui` files        |
| [`Lec15`](Lec15)  | Model/View, dialogs, and databases in PySide6                               | `students.py`, `products.py` + `products.csv`, `books.py`, `standard_views.py`                 |
| [`Lec16`](Lec16)  | Packaging, generic types, static analysis                                   | `units-project\` (installable package), `generics.py`, `bad.py`, `Budget\` (PyInstaller build) |

## Notes

- `Lec16\Budget\build_budget.py` builds the frozen `Budget` app with PyInstaller into `build-clean`
  and `dist-clean`.
- The virtual environment, caches, `.idea`, build output, `*.spec` and `*.db` are ignored by
  `.gitignore`.
