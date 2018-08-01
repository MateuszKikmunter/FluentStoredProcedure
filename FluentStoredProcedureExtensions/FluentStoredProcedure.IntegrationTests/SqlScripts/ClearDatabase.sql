-- Drop CreateEmployees Stored Procedure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.CreateEmployees') AND type in (N'P', N'PC'))
	DROP PROCEDURE CreateEmployees;

-- Drop GetEmployeeByName Stored Procedure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.GetEmployeeByName') AND type in (N'P', N'PC'))
	DROP PROCEDURE GetEmployeeByName;

-- Drop UpdateEmployees Stored Procedure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.UpdateEmployees') AND type in (N'P', N'PC'))
	DROP PROCEDURE UpdateEmployees;

-- Drop GetAllEmployees Stored Procedure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.GetAllEmployees') AND type in (N'P', N'PC'))
	DROP PROCEDURE GetAllEmployees;

-- Drop DeleteEmployeeById Stored Procedure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.DeleteEmployeeById') AND type in (N'P', N'PC'))
	DROP PROCEDURE DeleteEmployeeById;

-- Drop Employees Table
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employees' AND TABLE_SCHEMA = 'dbo')
    DROP TABLE dbo.Employees;

-- Drop Employee User Defined Table Type 
IF EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name ='EmployeeTableType')
	DROP TYPE [dbo].[EmployeeTableType];