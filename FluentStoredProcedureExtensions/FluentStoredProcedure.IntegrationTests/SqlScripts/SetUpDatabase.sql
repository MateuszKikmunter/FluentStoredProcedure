-- Create Employees Table

CREATE TABLE Employees (
    Id uniqueidentifier PRIMARY KEY,
    Name nvarchar(50) NOT NULL
);
	
ALTER TABLE Employees
ADD CONSTRAINT DF_Id DEFAULT newsequentialid() FOR Id

-- Create Employee User Defined Table Type

 CREATE TYPE [dbo].[EmployeeTableType] AS TABLE
(
    [Id] [uniqueidentifier] NULL,
    [Name] [nvarchar](50) NOT NULL
);

-- Create CreateEmployees Stored Procedure

GO
CREATE PROCEDURE dbo.CreateEmployees(@Employees dbo.EmployeeTableType READONLY)
AS
BEGIN
	INSERT INTO dbo.Employees (Name)
	SELECT Name FROM @Employees;
END

-- Create GetEmployeeByName Stored Procedure

GO
CREATE PROCEDURE dbo.GetEmployeeByName(@EmployeeName nvarchar(50))
AS
BEGIN
	SELECT * FROM Employees WHERE Name = @EmployeeName;
END

-- Create UpdateEmployees Stored Procedure

GO
CREATE PROCEDURE dbo.UpdateEmployees(@Employees dbo.EmployeeTableType READONLY)
AS
BEGIN
	UPDATE dbo.Employees 
	SET Employees.Name = e.Name  
    FROM dbo.Employees INNER JOIN @Employees AS e
    ON dbo.Employees.Id = e.Id;  
END

-- Create GetAllEmployees Stored Procedure

GO
CREATE PROCEDURE dbo.GetAllEmployees
AS
BEGIN
	SELECT * FROM Employees;
END

-- Create DeleteEmployeeById Stored Procedure

GO
CREATE PROCEDURE dbo.DeleteEmployeeById(@EmployeeId uniqueidentifier)
AS
BEGIN
	DELETE FROM Employees WHERE Id = @EmployeeId;
END