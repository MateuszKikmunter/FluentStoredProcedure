using System;
using FluentAssertions;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Data;
using Moq;
using NUnit.Framework;

namespace FluentStoredProcedureExtensions.UnitTests.DataServicesTests
{
    [TestFixture]
    public class StoredProcedureFactoryTests
    {
        private Mock<ISqlParameterFactory> _sqlParameterFactoryMock;
        private IStoredProcedureFactory _storedProcedureFactory;

        [SetUp]
        public void SetUp()
        {
            _sqlParameterFactoryMock = new Mock<ISqlParameterFactory>();
            _storedProcedureFactory = new StoredProcedureFactory(_sqlParameterFactoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _sqlParameterFactoryMock = null;
            _storedProcedureFactory = null;
        }

        [Test]
        public void CreateStoredProcedure_EmptyStringStoredProcedureName_ShouldThrow()
        {
             //arrange
            var storedProcedureName = string.Empty;

            //act
            Action result = () => _storedProcedureFactory.CreateStoredProcedure(storedProcedureName);

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CreateStoredProcedure_CorrectInput_ShouldCreateStoredProcedure()
        {
            //arrange
            var storedProcedureName = "uspHighPerformantStoredProcedure";

            //act
            var result = _storedProcedureFactory.CreateStoredProcedure(storedProcedureName);

            //assert
            result.Should().NotBeNull();
            result.StoredProcedureText.Should().Be(storedProcedureName);
        }
    }
}
