-- Відібрати групу, якій вимикають світло на найбільший час з понеділка по середу включно

SELECT TOP 1 s.GroupId, g.Name, 
    SUM(DATEDIFF(MINUTE, s.StartTime, s.FinishTime)) AS TotalOutageMinutes
FROM Schedule s
JOIN [Group] g ON s.GroupId = g.Id
WHERE s.Day IN ('Понеділок', 'Вівторок', 'Середа')
GROUP BY s.GroupId, g.Name
ORDER BY TotalOutageMinutes DESC;
