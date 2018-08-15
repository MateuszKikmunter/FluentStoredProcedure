﻿-- Create GetEmployee Stored Procedure

GO
CREATE PROCEDURE dbo.GetEmployee(@Id INT)
AS
BEGIN
	SELECT * FROM Employees WHERE Id = @Id;
END

-- Create UpdateEmployee Stored Procedure

GO
CREATE PROCEDURE dbo.UpdateEmployee(@Id INT, @Name NVARCHAR(155), @Email NVARCHAR(255))
AS
BEGIN
	UPDATE dbo.Employees
	SET Name = @Name, Email = @Email
	WHERE Id = @Id
END

-- Create GetAllEmployees Stored Procedure

GO
CREATE PROCEDURE dbo.GetAllEmployees
AS
BEGIN
	SELECT * FROM Employees;
END

-- Create DeleteEmployee Stored Procedure

GO
CREATE PROCEDURE dbo.DeleteEmployee(@Id INT)
AS
BEGIN
	DELETE FROM Employees WHERE Id = @Id;
END

-- Create GetEmployee Stored Procedure

GO
CREATE PROCEDURE dbo.GetEmployee(@Id INT)
AS
BEGIN
	SELECT * FROM dbo.Employees WHERE Id = @Id;
END