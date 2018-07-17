using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentStoredProcedureExtensions.Infrastructure.Services
{
    public static class Guard
    {
        public static void ThrowIfNull<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public static void ThrowIfCollectionNullOrEmpty<T>(IEnumerable<T> collection)
        {
            if (collection == null || !collection.Any())
            {
                throw new ArgumentException("Collection cannot be null or empty.", nameof(collection));
            }
        }

        public static void ThrowIfStringIsNullOrWhiteSpace(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(nameof(input));
            }
        }
    }
}
