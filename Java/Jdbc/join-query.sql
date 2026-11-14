-- Query of example 2 (CatalogMain.find) with the parameter value inlined.
-- In DataGrip choose schema j14_demo in the console's schema switcher, then Ctrl+Enter.
SELECT b.id, b.title, a.name AS author
FROM j14_books b JOIN j14_authors a ON a.id=b.author_id
WHERE a.name = 'Автор A' ORDER BY b.id;
