-- Disposable demo schema for screenshot 14/05 (DataGrip SQL console).
-- Run as role lab in database library (see lecture 14, «Підготовка PostgreSQL»).
-- Tables and rows are those of example 2 (CatalogMain), as persistent tables
-- in their own schema; rerunning the script recreates them from scratch.
-- Remove afterwards: DROP SCHEMA IF EXISTS j14_demo CASCADE;
DROP SCHEMA IF EXISTS j14_demo CASCADE;
CREATE SCHEMA j14_demo;
SET search_path TO j14_demo;

CREATE TABLE j14_authors (
  id bigint PRIMARY KEY, name text NOT NULL);
CREATE TABLE j14_books (
  id bigint PRIMARY KEY, title text NOT NULL,
  author_id bigint REFERENCES j14_authors(id));
INSERT INTO j14_authors VALUES (1, 'Автор A');
INSERT INTO j14_books VALUES
  (1, 'Весна', 1), (2, 'Літо', 1);
