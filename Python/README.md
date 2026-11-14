# Object-oriented programming in Python

Code examples for the course [Object-oriented programming in Python](https://mvano.com/courses/python/).
Each topic of the course has its own folder named after the topic; the table below lists them in
course order and links each topic page.

## Tools

Python 3.14 and PyCharm 2026.2 (or VS Code). Open this `Python` folder as the project and create one
virtual environment for all topics:

```powershell
py -3.14 -m venv .venv
.venv\Scripts\python -m pip install rich pytest sqlalchemy numpy pandas matplotlib PySide6 pyinstaller
.venv\Scripts\python ControlFlow\quadratic.py
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

| #                                                                  | Folder                             | Topic                                                                 | Contents                                                                                       |
| ------------------------------------------------------------------ | ---------------------------------- | --------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------- |
| [1](https://mvano.com/courses/python/01-intro/)                    | [`Intro`](Intro)                   | Python 3.14, PyCharm, virtual environments, packages, Git             | `hello\` (first project with `requirements.txt`), `args.py`, `environment.py`, `broken\`       |
| [2](https://mvano.com/courses/python/02-types-control-flow/)       | [`ControlFlow`](ControlFlow)       | Data types, variables, operations, console input/output, control flow | `quadratic.py`, `pep8_demo.py`, `pep8_names.py`                                                |
| [3](https://mvano.com/courses/python/03-functions/)                | [`Functions`](Functions)           | Functions: parameters, scope, recursion, type hints                   | `area.py`, `area_type_warning.py`, `format_price.py`, `factorial_broken.py`                    |
| [4](https://mvano.com/courses/python/04-exceptions-debugging/)     | [`Exceptions`](Exceptions)         | Exception handling and debugging                                      | `average.py`, `average_single.py`, `traceback_demo.py`                                         |
| [5](https://mvano.com/courses/python/05-collections/)              | [`Collections`](Collections)       | Built-in collections and the `collections` module                     | `journal.py` (group grade journal)                                                             |
| [6](https://mvano.com/courses/python/06-strings-regex/)            | [`Strings`](Strings)               | Strings, f-strings, template strings, regular expressions             | `phone.py`, `tstring.py`, `dates.txt`                                                          |
| [7](https://mvano.com/courses/python/07-iterators-decorators/)     | [`Iterators`](Iterators)           | Iterators, generators, lambdas, decorators                            | `fibonacci.py`, `retry_timer.py`                                                               |
| [8](https://mvano.com/courses/python/08-classes/)                  | [`Classes`](Classes)               | Classes and objects: attributes, methods, properties, encapsulation   | `bank_account.py`, `cart.py`                                                                   |
| [9](https://mvano.com/courses/python/09-inheritance-protocols/)    | [`Inheritance`](Inheritance)       | Inheritance, polymorphism, abstract classes, protocols                | `shapes.py`, `shapes_implement.py`                                                             |
| [10](https://mvano.com/courses/python/10-special-methods/)         | [`SpecialMethods`](SpecialMethods) | Special methods, operator overloading, data classes, enums            | `products.py`, `products_frozen.py`                                                            |
| [11](https://mvano.com/courses/python/11-modules-files-pytest/)    | [`Modules`](Modules)               | Modules and packages, files (pathlib, JSON, CSV), testing with pytest | `geometry\` package, `expenses.py` + tests, `grades.py`                                        |
| [12](https://mvano.com/courses/python/12-databases/)               | [`Databases`](Databases)           | Relational databases: SQL, `sqlite3`, SQLAlchemy ORM                  | `contacts.py`, `grades.py`, `authors_orm.py`, `school.sql`, `left_join.sql`                    |
| [13](https://mvano.com/courses/python/13-numpy-pandas-matplotlib/) | [`DataAnalysis`](DataAnalysis)     | NumPy, pandas, and Matplotlib                                         | `sales.py`, `sensor_report.py`                                                                 |
| [14](https://mvano.com/courses/python/14-pyside6-widgets/)         | [`Widgets`](Widgets)               | PySide6 GUI applications: widgets, layouts, signals and slots         | `hello.py`, `converter.py`, `registration.py`, `notepad.py`, `gallery.py`, `.ui` files         |
| [15](https://mvano.com/courses/python/15-model-view/)              | [`ModelView`](ModelView)           | Model/View, dialogs, and databases in PySide6                         | `students.py`, `products.py` + `products.csv`, `books.py`, `standard_views.py`                 |
| [16](https://mvano.com/courses/python/16-packaging-typing/)        | [`Packaging`](Packaging)           | Packaging, generic types, static analysis                             | `units-project\` (installable package), `generics.py`, `bad.py`, `Budget\` (PyInstaller build) |

## Notes

- `Packaging\Budget\build_budget.py` builds the frozen `Budget` app with PyInstaller into `build-clean`
  and `dist-clean`.
- The virtual environment, caches, `.idea`, build output, `*.spec` and `*.db` are ignored by
  `.gitignore`.
