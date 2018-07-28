using FluentStoredProcedureExtensions.Infrastructure.Services;

namespace FluentStoredProcedureExtensions.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveLastCharacter(this string input)
        {
            Guard.ThrowIfStringIsNullOrWhiteSpace(input);
            return input.Remove(input.Length - 1);
        }
    }
}
