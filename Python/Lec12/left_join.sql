SELECT s.name, COUNT(g.score), AVG(g.score)
FROM students AS s
LEFT JOIN grades AS g ON g.student_id=s.id
GROUP BY s.id, s.name
ORDER BY s.id;
