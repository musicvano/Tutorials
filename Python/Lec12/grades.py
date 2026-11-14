import sqlite3
from contextlib import closing


with closing(sqlite3.connect(":memory:", autocommit=True)) as db:
    db.execute("PRAGMA foreign_keys=ON")
    db.autocommit = False
    with db:
        db.execute("""CREATE TABLE students (
            id INTEGER PRIMARY KEY, name TEXT NOT NULL)""")
        db.execute("""CREATE TABLE grades (
            student_id INTEGER NOT NULL REFERENCES students(id),
            score INTEGER NOT NULL CHECK(score BETWEEN 0 AND 100))""")
        db.executemany("INSERT INTO students VALUES(?,?)",
                       [(1, "Олена"), (2, "Іван"), (3, "Марія")])
        db.executemany("INSERT INTO grades VALUES(?,?)",
                       [(1, 80), (1, 100), (2, 70)])
    query = """SELECT s.name, COUNT(g.score), AVG(g.score)
        FROM students AS s
        LEFT JOIN grades AS g ON g.student_id=s.id
        GROUP BY s.id, s.name
        ORDER BY s.id"""
    for name, count, average in db.execute(query):
        text = "немає" if average is None else f"{average:.1f}"
        print(name, count, text)
