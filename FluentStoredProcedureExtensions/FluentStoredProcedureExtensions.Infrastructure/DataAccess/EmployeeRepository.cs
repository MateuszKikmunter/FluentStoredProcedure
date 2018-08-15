using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Core.Entities;
using FluentStoredProcedureExtensions.Infrastructure.DataAccess.Contexts;
using FluentStoredProcedureExtensions.Infrastructure.Extensions;

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

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.FromSqlAsync<Employee>(_storedProcedureFactory.CreateStoredProcedure("GetAllEmployees"));
        }

        public async Task<Employee> GetSingleAsync(int id)
        {
            var procedure = _storedProcedureFactory.CreateStoredProcedure("GetEmployee").WithSqlParam("Id", id);
            var result = await _context.FromSqlAsync<Employee>(procedure);
            return result.FirstOrDefault();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var procedure = _storedProcedureFactory.CreateStoredProcedure("DeleteEmployee").WithSqlParam("Id", id);
            var rowsAffected = await _context.ExecuteSqlCommandAsync(procedure);
            return rowsAffected > 0;
        }

        public async Task<Employee> UpdateAsync(int id, Employee employee)
        {
            var procedure = _storedProcedureFactory.CreateStoredProcedure("UpdateEmployee")
                .WithSqlParam("Id", id)
                .WithSqlParam("Name", employee.Name)
                .WithSqlParam("Email", employee.Email);

            await _context.ExecuteSqlCommandAsync(procedure);

            return _context.Employees.Find(id);
        }

        public async Task<bool> CreateAsync(Employee employee)
        {
            var procedure = _storedProcedureFactory.CreateStoredProcedure("CreateEmployee")
                .WithSqlParam("Name", employee.Name)
                .WithSqlParam("Email", employee.Email);

            var rowsAffected = await _context.ExecuteSqlCommandAsync(procedure);
            return rowsAffected > 0;
        }
    }
}
