using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentStoredProcedure.IntegrationTests.Context;
using FluentStoredProcedure.IntegrationTests.Entities;
using FluentStoredProcedure.IntegrationTests.Helpers;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Data;
using FluentStoredProcedureExtensions.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace FluentStoredProcedure.IntegrationTests.Tests
{
    [TestClass]
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

        [Test]
        public void WithSqlParam_ShouldExecuteStoredProcedureAndReturnEntities()
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
