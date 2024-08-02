-- Відібрати адреси, яким не призначено групу

SELECT * FROM Address
WHERE GroupId IS NULL;
