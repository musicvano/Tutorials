-- school.db for screenshots 12/03 (sqlite3 CLI), 12/04 and 12/08 (DataGrip):
-- the students/grades schema and data of lecture 12, example 2.
PRAGMA foreign_keys=ON;
CREATE TABLE students (
    id INTEGER PRIMARY KEY, name TEXT NOT NULL);
CREATE TABLE grades (
    student_id INTEGER NOT NULL REFERENCES students(id),
    score INTEGER NOT NULL CHECK(score BETWEEN 0 AND 100));
INSERT INTO students VALUES(1, 'Олена'), (2, 'Іван'), (3, 'Марія');
INSERT INTO grades VALUES(1, 80), (1, 100), (2, 70);
