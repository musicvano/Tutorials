-- Role lab and database library of lecture 14 («Підготовка PostgreSQL»).
-- Run by the owner of the local server:  psql -U postgres -f setup-library.sql
-- \password asks for the new password of lab twice (not echoed, not stored here).
CREATE ROLE lab LOGIN;
\password lab
CREATE DATABASE library OWNER lab;
