-- The separate database library and its own login role of
-- lecture 14 («Серверна альтернатива: PostgreSQL»), the same as Java\Jdbc –
-- skip this if that script has already been run on this server.
-- Run by the owner of the local server:  psql -U postgres -f setup-library.sql
-- \password asks for the new password of lab twice (not echoed, not stored here).
CREATE ROLE lab LOGIN;
\password lab
CREATE DATABASE library OWNER lab;
