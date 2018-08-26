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
    public class StoredProcedureTests
    {
        private Mock<ISqlParameterFactory> _mockSqlParameterFactory;
        private IStoredProcedure _storedProcedure;

        [SetUp]
        public void SetUp()
        {
            _mockSqlParameterFactory = new Mock<ISqlParameterFactory>();
            _storedProcedure = new StoredProcedure(_mockSqlParameterFactory.Object);
            _storedProcedure.SetStoredProcedureText("uspHighPerformatStoredProcedure");

            _mockSqlParameterFactory.Setup(factory => factory.CreateParameter(It.IsAny<string>()))
                .Returns(new SqlParameter("@Parameter", 123));

            _mockSqlParameterFactory.Setup(factory => factory.CreateParameter(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(new SqlParameter("@Parameter", "param-value"));

            _mockSqlParameterFactory.Setup(factory =>
                    factory.BuildUserDefinedTableTypeParameter(It.IsAny<string>(), It.IsAny<IList<FakeEmployee>>()))
                .Returns(new SqlParameter("@Parameter", FakeDataTable.GetDataTable()));
        }

        [TearDown]
        public void TearDown()
        {
            _mockSqlParameterFactory = null;
            _storedProcedure = null;
        }

        [Test]
        public void SetStoredProcedureText_EmptyString_ShouldThrow()
        {
            //arrange
            var text = string.Empty;
            
            //act/assert
            Assert.Throws<ArgumentException>(() => _storedProcedure.SetStoredProcedureText(text));
        }

        [Test]
        public void SetStoredProcedureText_CorrectInput_ShouldSetStoredProcedureText()
        {
            //arrange
            var text = "my-super-duper-stored-procedure";

            //act
            _storedProcedure.SetStoredProcedureText(text);

            //assert
            _storedProcedure.StoredProcedureText.Should().Be(text);
        }

        [Test]
        public void WithSqlParam_EmptyStringParameterName_ShouldThrow()
        {
            //arrange
            var parameterName = string.Empty;

            //act
            Action result = () => _storedProcedure.WithSqlParam(parameterName);

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void WithSqlParam_CorrectInputNoAdditionalConfig_ShouldReturnNewStoredProcureWithCreatedParameter()
        {
            //arrange
            var parameterName = "Parameter";

            //act
            var result = _storedProcedure.WithSqlParam(parameterName);

            //assert
            result.StoredProcedureText.Should().Be("uspHighPerformatStoredProcedure @Parameter");
            result.SqlParametersCollection.Should().NotBeNullOrEmpty();
            result.SqlParametersCollection.First().ParameterName.Should().Be("@Parameter");
            result.SqlParametersCollection.First().Value.Should().Be(123);
        }

        [Test]
        public void WithSqlParam_CorrectInputMultipleParametersNoAdditionalConfig_ShouldReturnNewStoredProcureWithCreatedParameter()
        {
            //arrange
            var parameterName = "Parameter";

            //act
            var result = _storedProcedure
                .WithSqlParam(parameterName)
                .WithSqlParam(parameterName)
                .WithSqlParam(parameterName);

            //assert
            result.StoredProcedureText.Should().Be("uspHighPerformatStoredProcedure @Parameter,@Parameter,@Parameter");
            result.SqlParametersCollection.Should().NotBeNullOrEmpty();
            result.SqlParametersCollection.Count.Should().Be(3);
            result.SqlParametersCollection.Should().OnlyContain(p => p.ParameterName.Equals("@Parameter"));
            result.SqlParametersCollection.Should().OnlyContain(p => (int)p.Value == 123);
        }

        [Test]
        public void WithSqlParam_AdditionalConfiguration_ShouldCreateParameters()
        {
            //arrange
            var parameterValue = "nevermind-it-comes-from-factory-mock";

            //act
            var result = _storedProcedure
                .WithSqlParam("Parameter", parameterValue, parameter =>
                {
                    parameter.Direction = ParameterDirection.Output;
                    parameter.DbType = DbType.String;
                    parameter.Size = 100;
                })
                .WithSqlParam("Parameter", parameterValue);

            //assert
            result.StoredProcedureText.Should().Be("uspHighPerformatStoredProcedure @Parameter,@Parameter");

            result.SqlParametersCollection.Count.Should().Be(2);

            result.SqlParametersCollection[0].ParameterName.Should().Be("@Parameter");
            result.SqlParametersCollection[0].Direction.Should().Be(ParameterDirection.Output);
            result.SqlParametersCollection[0].DbType.Should().Be(DbType.String);
            result.SqlParametersCollection[0].Size.Should().Be(100);

            result.SqlParametersCollection[1].ParameterName.Should().Be("@Parameter");
            result.SqlParametersCollection[1].Value.Should().Be("param-value");
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_EmptyStringAsParamName_ShouldThrow()
        {
            //arrange
            var paramName = string.Empty;

            //act
            Action result = () => _storedProcedure.WithUserDefinedDataTableSqlParam(paramName, new List<string>());

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_EmptyCollectionAsParamValue_ShouldThrow()
        {
            //arrange
            var paramName = "blah";

            //act
            Action result = () => _storedProcedure.WithUserDefinedDataTableSqlParam(paramName, new List<string>());

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void WithUserDefinedDataTableSqlParam_CorrectInput_ShouldCreateParameter()
        {
            //arrange
            var paramName = "Parameter";

            //act
            var result = _storedProcedure.WithUserDefinedDataTableSqlParam(paramName, FakeCollectionData.GetTestData());

            //assert
            result.Should().NotBeNull();
            result.StoredProcedureText.Should().Be("uspHighPerformatStoredProcedure @Parameter");
            result.SqlParametersCollection[0].ParameterName.Should().Be("@Parameter");
            result.SqlParametersCollection.Count.Should().Be(1);

            var parameterValueAsDataTable = result.SqlParametersCollection[0].Value as DataTable;
            parameterValueAsDataTable.Rows.Count.Should().Be(2);
            parameterValueAsDataTable.Columns.Count.Should().Be(2);

            parameterValueAsDataTable.Columns[0].ColumnName.Should().Be("Id");
            parameterValueAsDataTable.Columns[1].ColumnName.Should().Be("Name");

        }
    }
}
