-- Відібрати графік відключень світла для адреси Бойченко 30

SELECT s.*
FROM Schedule s
JOIN Address a ON s.GroupId = a.GroupId
WHERE a.Address = 'Бойченко 30';
