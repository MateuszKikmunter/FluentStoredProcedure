using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentStoredProcedure.IntegrationTests.Context;
using FluentStoredProcedure.IntegrationTests.Entities;
using FluentStoredProcedure.IntegrationTests.Helpers;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Data;
using FluentStoredProcedureExtensions.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FluentStoredProcedure.IntegrationTests.Tests
{
    [TestFixture]
    public class StoredProcedureTests
    {
        private ApplicationDbContext _context;
        private IStoredProcedureFactory _storedProcedureFactory;
        private ISqlParameterFactory _sqlParameterFactory;

        [SetUp]
        public void SetUp()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(ConnectionStringProvider.GetConnectionString());

            _context = new ApplicationDbContext(builder.Options);
            _sqlParameterFactory = new SqlParameterFactory(new CollectionToDataTableConverter());
            _storedProcedureFactory = new StoredProcedureFactory(_sqlParameterFactory);

            SqlScriptRunner.SetUpDatabase();
            SeedDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            _sqlParameterFactory = null;
            _storedProcedureFactory = null;
            SqlScriptRunner.ClearDatabase();
        }

        #region Synchronous Code
        [Test]
        public void WithSqlParam_Get_ShouldExecuteStoredProcedureAndReturnEntities()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("GetEmployeeByName");

            //act

            storedProcedure.WithSqlParam("EmployeeName", "Luke Skywalker");
            var result = _context.FromSql<Employee>(storedProcedure);

            //assert

            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(1);
            result.First().Name.Should().Be("Luke Skywalker");
        }

        [Test]
        public void Get_StoredProcedureWithoutParameters_ShouldExecuteStoredProcedureAndReturnEntities()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("GetAllEmployees");

            //act
            var result = _context.FromSql<Employee>(storedProcedure);

            //assert

            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(2);
        }

        [Test]
        public void WithSqlParam_Delete_ShouldExecuteStoredProcedureAndReturnNumberOfAffectedRows()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("DeleteEmployeeById");

            //act
            var employeeToDelete = _context.Employees.First();
            storedProcedure.WithSqlParam("EmployeeId", employeeToDelete.Id);
            var rowsAffected = _context.ExecuteSqlCommand(storedProcedure);

            //assert
            rowsAffected.Should().Be(1);
            _context.Employees.ToList().Count.Should().Be(1);
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_Update_ShouldExecuteStoredProcedureAndReturnNumberOfAffectedRows()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("UpdateEmployees");

            //act
            var employees = _context.Employees.ToList();
            employees.First().Name = "Chewbacca";
            employees.Last().Name = "Han Solo";

            storedProcedure.WithUserDefinedDataTableSqlParam("Employees", employees, param => param.TypeName = "dbo.EmployeeTableType");

            var rowsAffected = _context.ExecuteSqlCommand(storedProcedure);
            var employeesAfterUpdate = _context.Employees.ToList();

            //assert
            rowsAffected.Should().Be(2);
            employeesAfterUpdate.First().Name.Should().Be(employees.First().Name);
            employeesAfterUpdate.Last().Name.Should().Be(employees.Last().Name);
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_Create_ShouldExecuteStoredProcedureAndReturnNumberOfAffectedRows()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("CreateEmployees");
            var employeesToAdd = new List<Employee>
            {
                new Employee
                {
                    Name = "Tom"
                },
                new Employee
                {
                    Name = "Jerry"
                }
            };

            //act
            storedProcedure.WithUserDefinedDataTableSqlParam("Employees", employeesToAdd, param => param.TypeName = "dbo.EmployeeTableType");
            var rowsAffected = _context.ExecuteSqlCommand(storedProcedure);

            //assert
            rowsAffected.Should().Be(2);
            _context.Employees.ToList().Count.Should().Be(4);
        }
        #endregion

        #region Async Code

        [Test]
        public async Task WithSqlParam_GetAsync_ShouldExecuteStoredProcedureAndReturnEntities()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("GetEmployeeByName");

            //act

            storedProcedure.WithSqlParam("EmployeeName", "Luke Skywalker");
            var result = await _context.FromSqlAsync<Employee>(storedProcedure);

            //assert

            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(1);
            result.First().Name.Should().Be("Luke Skywalker");
        }

        [Test]
        public async Task GetAsync_StoredProcedureWithoutParameters_ShouldExecuteStoredProcedureAndReturnEntities()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("GetAllEmployees");

            //act
            var result = await _context.FromSqlAsync<Employee>(storedProcedure);

            //assert

            result.Should().NotBeNullOrEmpty();
            result.Count().Should().Be(2);
        }

        [Test]
        public async Task WithSqlParam_DeleteAsync_ShouldExecuteStoredProcedureAndReturnNumberOfAffectedRows()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("DeleteEmployeeById");

            //act
            var employeeToDelete = _context.Employees.First();
            storedProcedure.WithSqlParam("EmployeeId", employeeToDelete.Id);
            var rowsAffected = await _context.ExecuteSqlCommandAsync(storedProcedure);

            //assert
            rowsAffected.Should().Be(1);
            _context.Employees.ToList().Count.Should().Be(1);
        }

        [Test]
        public async Task WithUserDefinedDataTableSqlParam_UpdateAsync_ShouldExecuteStoredProcedureAndReturnNumberOfAffectedRows()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("UpdateEmployees");

            //act
            var employees = _context.Employees.ToList();
            employees.First().Name = "Chewbacca";
            employees.Last().Name = "Han Solo";

            storedProcedure.WithUserDefinedDataTableSqlParam("Employees", employees, param => param.TypeName = "dbo.EmployeeTableType");

            var rowsAffected = await _context.ExecuteSqlCommandAsync(storedProcedure);
            var employeesAfterUpdate = _context.Employees.ToList();

            //assert
            rowsAffected.Should().Be(2);
            employeesAfterUpdate.First().Name.Should().Be(employees.First().Name);
            employeesAfterUpdate.Last().Name.Should().Be(employees.Last().Name);
        }

        [Test]
        public async Task WithUserDefinedDataTableSqlParam_CreateAsync_ShouldExecuteStoredProcedureAndReturnNumberOfAffectedRows()
        {
            //arrange
            var storedProcedure = _storedProcedureFactory.CreateStoredProcedure("CreateEmployees");
            var employeesToAdd = new List<Employee>
            {
                new Employee
                {
                    Name = "Tom"
                },
                new Employee
                {
                    Name = "Jerry"
                }
            };

            //act
            storedProcedure.WithUserDefinedDataTableSqlParam("Employees", employeesToAdd, param => param.TypeName = "dbo.EmployeeTableType");
            var rowsAffected = await _context.ExecuteSqlCommandAsync(storedProcedure);

            //assert
            rowsAffected.Should().Be(2);
            _context.Employees.ToList().Count.Should().Be(4);
        }

        #endregion

        private void SeedDatabase()
        {
            var entitiesToInsert = new List<Employee>
            {
                new Employee
                {
                    Name = "Luke Skywalker"
                },
                new Employee
                {
                    Name = "Darth Vader"
                }
            };

            _context.Employees.AddRange(entitiesToInsert);
            _context.SaveChanges();
        }
    }
}
