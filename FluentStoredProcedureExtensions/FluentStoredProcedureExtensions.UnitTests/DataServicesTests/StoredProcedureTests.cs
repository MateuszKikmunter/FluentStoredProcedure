using System;
using System.Data.SqlClient;
using System.Linq;
using FluentAssertions;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Data;
using Moq;
using NUnit.Framework;

namespace FluentStoredProcedureExtensions.UnitTests.DataServicesTests
{
    [TestFixture]
    public class StoredProcedureTests
    {
        private Mock<ISqlParameterFactory> _mockSqlParameterFactory;
        private Mock<IStoredProcedureFactory> _mockStoredProcedureFactory;
        private IStoredProcedure _storedProcedure;

        [SetUp]
        public void SetUp()
        {
            _mockSqlParameterFactory = new Mock<ISqlParameterFactory>();
            _mockStoredProcedureFactory = new Mock<IStoredProcedureFactory>();
            _storedProcedure = new StoredProcedure(_mockSqlParameterFactory.Object);
            _storedProcedure.SetStoredProcedureText("uspHighPerformatStoredProcedure");

            _mockSqlParameterFactory.Setup(factory => factory.CreateParameter(It.IsAny<string>()))
                .Returns(new SqlParameter("@Parameter", 123));  
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
            var text = "uspHighPerformantStoredProcedure";

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
            result.SqlParametersCollection.Select(p => p.ParameterName.Should().Be("@Parameter"));
            result.SqlParametersCollection.Select(p => p.Value.Should().Be(123));
        }
    }
}
