using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Infrastructure.DataAccess.Contexts;

namespace FluentStoredProcedureExtensions.Infrastructure.DataAccess
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IStoredProcedureFactory _storedProcedureFactory;
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(IStoredProcedureFactory storedProcedureFactory, ApplicationDbContext context)
        {
            _storedProcedureFactory = storedProcedureFactory;
            _context = context;
        }
    }
}
