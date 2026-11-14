-- Read-only queries of Lecture 07 (run after library.sql).

SELECT title, pub_year               -- які стовпці
FROM books                           -- з якої таблиці
WHERE title ILIKE '%clean%' AND pub_year >= 2010  -- які рядки
ORDER BY pub_year DESC               -- порядок
LIMIT 10 OFFSET 0;                   -- 10 рядків, пропустити 0

SELECT count(*), min(pub_year), max(pub_year) FROM books;

SELECT a.name, count(*) AS books
FROM authors AS a
JOIN book_authors AS ba ON ba.author_id = a.id
GROUP BY a.name
HAVING count(*) > 1
ORDER BY a.name;

SELECT b.title, a.name AS author, b.pub_year
FROM books AS b
JOIN book_authors AS ba ON ba.book_id = b.id
JOIN authors AS a ON a.id = ba.author_id
ORDER BY b.title, a.name;

-- Screenshot 03-datagrip-query-console.png
SELECT r.name,
       count(l.id) AS loans,
       count(l.id) FILTER (WHERE l.returned IS NULL) AS on_hand
FROM readers AS r
LEFT JOIN loans AS l ON l.reader_id = r.id
GROUP BY r.id, r.name
ORDER BY loans DESC, r.name;

SELECT r.name, b.title, l.due,
       DATE '2026-09-20' - l.due AS days_late
FROM loans AS l
JOIN readers AS r ON r.id = l.reader_id
JOIN books AS b ON b.id = l.book_id
WHERE l.returned IS NULL AND l.due < DATE '2026-09-20'
ORDER BY days_late DESC;
