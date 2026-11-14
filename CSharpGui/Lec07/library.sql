DROP TABLE IF EXISTS loans, book_authors, readers, books, authors;

CREATE TABLE authors (
    id   integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name varchar(100) NOT NULL
);

CREATE TABLE books (
    id       integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    title    varchar(200) NOT NULL,
    pub_year integer CHECK (pub_year BETWEEN 1450 AND 2100),
    isbn     varchar(17) UNIQUE,
    copies   integer NOT NULL DEFAULT 1 CHECK (copies >= 0)
);

-- Зв’язок N:M: складений первинний ключ із двох зовнішніх.
CREATE TABLE book_authors (
    book_id   integer REFERENCES books (id) ON DELETE CASCADE,
    author_id integer REFERENCES authors (id) ON DELETE RESTRICT,
    PRIMARY KEY (book_id, author_id)
);

CREATE TABLE readers (
    id            integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name          varchar(100) NOT NULL,
    email         text NOT NULL UNIQUE,
    active        boolean NOT NULL DEFAULT true,
    registered_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE loans (
    id        bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    book_id   integer NOT NULL REFERENCES books (id),
    reader_id integer NOT NULL REFERENCES readers (id),
    issued    date NOT NULL DEFAULT CURRENT_DATE,
    due       date NOT NULL,
    returned  date,
    CHECK (due >= issued)
);

CREATE INDEX ix_loans_reader_id ON loans (reader_id);

INSERT INTO authors (name)
VALUES ('Brian Kernighan'), ('Dennis Ritchie'),
       ('Martin Fowler'), ('Robert Martin');

INSERT INTO books (title, pub_year, isbn, copies)
VALUES ('The C Programming Language', 1988, '978-0131103627', 2),
       ('Refactoring', 2018, '978-0134757599', 3),
       ('Clean Code', 2008, '978-0132350884', 1),
       ('Clean Architecture', 2017, '978-0134494166', 0),
       ('UML Distilled', 2003, '978-0321193681', 2);

INSERT INTO book_authors (book_id, author_id)
VALUES (1, 1), (1, 2), (2, 3), (3, 4), (4, 4), (5, 3);

INSERT INTO readers (name, email)
VALUES ('Olena Kovalenko', 'olena@example.com'),
       ('Petro Shevchenko', 'petro@example.com'),
       ('Iryna Bondar', 'iryna@example.com')
RETURNING id, name;

INSERT INTO loans (book_id, reader_id, issued, due, returned)
VALUES (1, 1, '2026-09-01', '2026-09-15', '2026-09-10'),
       (2, 1, '2026-09-05', '2026-09-19', NULL),
       (3, 2, '2026-09-02', '2026-09-16', NULL),
       (2, 2, '2026-09-10', '2026-09-24', NULL),
       (5, 1, '2026-09-12', '2026-09-26', NULL);
