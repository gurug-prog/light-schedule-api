-- Відібрати групу, якій найчастіше вимикають світло в неділю

SELECT TOP 1 s.GroupId, g.Name, COUNT(*) AS CountOfOutages
FROM Schedule s
JOIN [Group] g ON s.GroupId = g.Id
WHERE s.Day = 'Неділя'
GROUP BY s.GroupId, g.Name
ORDER BY CountOfOutages DESC;
