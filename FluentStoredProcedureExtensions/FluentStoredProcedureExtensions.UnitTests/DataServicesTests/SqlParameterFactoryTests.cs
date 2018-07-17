using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FluentAssertions;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Data;
using FluentStoredProcedureExtensions.UnitTests.Helpers;
using Moq;
using NUnit.Framework;

namespace FluentStoredProcedureExtensions.UnitTests.DataServicesTests
{
    [TestFixture]
    public class SqlParameterFactoryTests
    {
        private ISqlParameterFactory _sqlParameterFactory;
        private Mock<ICollectionToDataTableConverter> _mockCollectionToDataTableConverter;

        [SetUp]
        public void Setup()
        {
            _mockCollectionToDataTableConverter = new Mock<ICollectionToDataTableConverter>();
            _sqlParameterFactory = new SqlParameterFactory(_mockCollectionToDataTableConverter.Object);

            _mockCollectionToDataTableConverter
                .Setup(converter => converter.ConvertToDataTable(It.IsAny<IList<FakeEmployee>>()))
                .Returns(GetDataTableResult());
        }

        [TearDown]
        public void TearDown()
        {
            _mockCollectionToDataTableConverter = null;
            _sqlParameterFactory = null;
        }

        [Test]
        public void CreateParameter_EmptyStringParameterName_ShouldThrow()
        {
            //arrange
            var input = string.Empty;

            //act
            Action result = () => _sqlParameterFactory.CreateParameter(input, null);

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CreateParameter_NullParameter_ShouldThrow()
        {
            //arrange
            var parameterName = "Param";

            //act
            Action result = () => _sqlParameterFactory.CreateParameter(parameterName, null);

            //assert
            result.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void CreateParameter_ValidInput_ShouldCreateSqlParameter()
        {
            //arrange
            var parameterName = "Param";
            var parameterValue = 123;
            var exptected = new SqlParameter($"@{parameterName}", parameterValue);

            //act
            var result = _sqlParameterFactory.CreateParameter(parameterName, parameterValue);

            //assert
            result.ParameterName.Should().Be(exptected.ParameterName);
            result.SqlValue.Should().Be(exptected.SqlValue);
        }

        [Test]
        public void CreateParameter_ParameterNameOnlySignature_ParameterNameEmptyString_ShouldThrow()
        {
            //arrange
            var input = string.Empty;

            //act
            Action result = () => _sqlParameterFactory.CreateParameter(input);

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CreateParameter_ParameterNameOnlySignature_ValidInput_ShouldCreateSqlParameter()
        {
            //arrange
            var parameterName = "Param";
            var expected = new SqlParameter { ParameterName = $"@{parameterName}" };

            //act
            var result = _sqlParameterFactory.CreateParameter(parameterName);

            //assert
            result.ParameterName.Should().Be(expected.ParameterName);
        }

        [Test]
        public void BuildUserDefinedTableTypeParameter_EmptyString_ShouldThrow()
        {
            //arrange
            var input = string.Empty;

            //act
            Action result = () => _sqlParameterFactory.BuildUserDefinedTableTypeParameter(input, new List<string>());

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void BuildUserDefinedTableTypeParameter_NullCollection_ShouldThrow()
        {
            //arrange
            var input = "Param";

            //act
            Action result = () => _sqlParameterFactory.BuildUserDefinedTableTypeParameter<IList<string>>(input, null);

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void BuildUserDefinedTableTypeParameter_EmptyCollection_ShouldThrow()
        {
            //arrange
            var input = "Param";

            //act
            Action result = () => _sqlParameterFactory.BuildUserDefinedTableTypeParameter(input, new List<string>());

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void BuildUserDefinedTableTypeParameter_ValidInput_ShouldCreateSqlParameter()
        {
            //arrange
            var parameterName = "FalconMilleniumCrew";
            var parameterValue = GetTestData();
            //act
            var result = _sqlParameterFactory.BuildUserDefinedTableTypeParameter(parameterName, parameterValue);
            var parameterValueAsDataTable = result.Value as DataTable;

            //assert
            result.ParameterName.Should().Be($"@{parameterName}");
            result.SqlDbType.Should().Be(SqlDbType.Structured);

            parameterValueAsDataTable.Rows.Count.Should().Be(2);
            parameterValueAsDataTable.Columns.Count.Should().Be(2);

            parameterValueAsDataTable.Columns[0].ColumnName.Should().Be("Id");
            parameterValueAsDataTable.Columns[1].ColumnName.Should().Be("Name");
        }

        private DataTable GetDataTableResult()
        {
            var tableData = GetTestData();
            var result = new DataTable();

            result.Columns.Add("Id", typeof(int));
            result.Columns.Add("Name", typeof(string));
            result.Rows.Add(tableData.First().Id, tableData.First().Name);
            result.Rows.Add(tableData.Last().Id, tableData.Last().Name);

            return result;
        }

        private List<FakeEmployee> GetTestData()
        {
            return new List<FakeEmployee>
            {
                new FakeEmployee { Id = 1, Name = "Chewbacca" },
                new FakeEmployee { Id = 2, Name = "Han Solo" }
            };
        }
    }
}
