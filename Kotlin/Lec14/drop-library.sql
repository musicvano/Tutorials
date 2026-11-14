-- Removes the database library and the role lab (to repeat screenshot 14/03).
-- Run by the owner of the local server:  psql -U postgres -f drop-library.sql
DROP DATABASE IF EXISTS library;
DROP ROLE IF EXISTS lab;
