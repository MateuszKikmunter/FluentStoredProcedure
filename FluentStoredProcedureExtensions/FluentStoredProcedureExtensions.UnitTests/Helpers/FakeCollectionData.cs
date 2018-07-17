using System.Collections.Generic;

namespace FluentStoredProcedureExtensions.UnitTests.Helpers
{
    public static class FakeCollectionData
    {
        public static IList<FakeEmployee> GetTestData()
        {
            return new List<FakeEmployee>
            {
                new FakeEmployee { Id = 1, Name = "Chewbacca" },
                new FakeEmployee { Id = 2, Name = "Han Solo" }
            };
        }
    }
}
