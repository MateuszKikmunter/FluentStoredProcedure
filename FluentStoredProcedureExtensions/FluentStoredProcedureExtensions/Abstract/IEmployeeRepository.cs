using System.Collections.Generic;
using System.Threading.Tasks;
using FluentStoredProcedureExtensions.Core.Entities;

namespace FluentStoredProcedureExtensions.Core.Abstract
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetSingleAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<Employee> UpdateAsync(int id, Employee employee);
        Task<bool> CreateAsync(Employee employee);
    }
}
