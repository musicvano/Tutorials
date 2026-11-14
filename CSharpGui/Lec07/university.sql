-- Lab 07, example 2 «Успішність студентів»: database university.

CREATE TABLE students (
    id         integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name       varchar(100) NOT NULL,
    group_name varchar(10) NOT NULL
);

CREATE TABLE grades (
    id         bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    student_id integer NOT NULL
               REFERENCES students (id) ON DELETE CASCADE,
    subject    varchar(50) NOT NULL,
    score      integer NOT NULL CHECK (score BETWEEN 0 AND 100),
    UNIQUE (student_id, subject)
);

INSERT INTO students (name, group_name)
VALUES ('Anna Petrenko', 'CS-24'), ('Bohdan Hnatiuk', 'CS-24'),
       ('Viktor Zinchenko', 'CS-24'), ('Halyna Rudenko', 'CS-25');

INSERT INTO grades (student_id, subject, score)
VALUES (1, 'Math', 92), (1, 'Programming', 88), (1, 'Physics', 95),
       (2, 'Math', 60), (2, 'Programming', 75), (2, 'Physics', 54),
       (4, 'Math', 81);
