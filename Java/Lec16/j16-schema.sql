-- Disposable schema for lecture 16, example 4 (BookMain) and the FXML variant
-- (screenshots 16/10, 16/11). Run in the course database as the role the
-- programs connect with (lecture 14: role lab, database library; the DataGrip
-- data source of 14/05 is postgres@localhost), e.g. in a DataGrip console or
--   psql -h localhost -U lab -d library -f j16-schema.sql
-- The CREATE TABLE is the lecture listing; the programs use the unqualified
-- name j16_books, so the connection selects the schema:
--   COURSE_DB_URL=jdbc:postgresql://localhost:5432/library?currentSchema=j16_demo
-- Rerunning the script recreates the table empty (books are added in the GUI).
-- Remove afterwards: DROP SCHEMA IF EXISTS j16_demo CASCADE;
DROP SCHEMA IF EXISTS j16_demo CASCADE;
CREATE SCHEMA j16_demo;
SET search_path TO j16_demo;

CREATE TABLE j16_books (
  id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  title varchar(100) NOT NULL CHECK (length(trim(title)) > 0),
  year integer NOT NULL CHECK (year BETWEEN 1450 AND 2100)
);
