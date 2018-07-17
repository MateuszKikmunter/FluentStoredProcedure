namespace FluentStoredProcedureExtensions.Core.Abstract
{
    public interface IStoredProcedureFactory
    {
        IStoredProcedure CreateStoredProcedure(string storedProcedureName);
    }
}
