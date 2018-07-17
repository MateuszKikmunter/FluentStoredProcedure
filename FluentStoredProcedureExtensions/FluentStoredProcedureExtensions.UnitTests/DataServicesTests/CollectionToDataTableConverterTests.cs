using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.Data;
using FluentStoredProcedureExtensions.UnitTests.Helpers;
using NUnit.Framework;

namespace FluentStoredProcedureExtensions.UnitTests.DataServicesTests
{
    [TestFixture]
    public class CollectionToDataTableConverterTests
    {
        private ICollectionToDataTableConverter _collectionToDataTableConverter;

        [SetUp]
        public void SetUp() => _collectionToDataTableConverter = new CollectionToDataTableConverter();

        [TearDown]
        public void TearDown() => _collectionToDataTableConverter = null;

        [Test]
        public void ConvertToDataTable_NullCollection_ShouldThrow()
        {
            Action result = () => _collectionToDataTableConverter.ConvertToDataTable<IList<FakeEmployee>>(null);

            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ConvertToDataTable_EmptyCollection_ShouldThrow()
        {
            //arrange
            var collection = new List<FakeEmployee>();

            //act
            Action result = () => _collectionToDataTableConverter.ConvertToDataTable(collection);

            //assert
            result.Should().Throw<ArgumentException>();
        }

        [Test]
        public void ConvertToDataTable_ValidCollection_ShouldReturnDataTableFilledWithData()
        {
            //arrange
            var collection = FakeCollectionData.GetTestData();

            //act
            var result = _collectionToDataTableConverter.ConvertToDataTable(collection);

            //assert

            // check rows and columns count
            result.Rows.Count.Should().Be(2);
            result.Columns.Count.Should().Be(2);

            //check columns names
            result.Columns[0].ColumnName.Should().Be("Id");
            result.Columns[1].ColumnName.Should().Be("Name");

            //check rows values
            result.Rows[0].ItemArray[0].Should().Be(collection.First().Id);
            result.Rows[0].ItemArray[1].Should().Be(collection.First().Name);

            result.Rows[1].ItemArray[0].Should().Be(collection.Last().Id);
            result.Rows[1].ItemArray[1].Should().Be(collection.Last().Name);
        }
    }
}
