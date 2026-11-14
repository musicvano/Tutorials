import sqlite3
from contextlib import closing


with closing(sqlite3.connect(":memory:", autocommit=True)) as db:
    db.execute("PRAGMA foreign_keys=ON")
    db.autocommit = False
    db.row_factory = sqlite3.Row
    with db:
        db.execute("""CREATE TABLE contacts (
            id INTEGER PRIMARY KEY,
            name TEXT NOT NULL CHECK(length(trim(name)) > 0),
            phone TEXT NOT NULL UNIQUE) STRICT""")
        db.executemany("INSERT INTO contacts(name,phone) VALUES(?,?)",
                       [("Олена", "0123"), ("Іван", "0456")])
    with db:
        db.execute("UPDATE contacts SET phone=? WHERE name=?",
                   ("0789", "Олена"))
        db.execute("DELETE FROM contacts WHERE name=?", ("Іван",))
    for row in db.execute("SELECT name,phone FROM contacts"):
        print(row["name"], row["phone"])
