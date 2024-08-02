-- Відібрати адреси, яким не призначено групу

SELECT * FROM Addresses
WHERE GroupId IS NULL;
