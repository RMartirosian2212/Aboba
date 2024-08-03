using Aboba.Domain.Entities;

namespace Aboba.Application.Interfaces;

public interface IEmployeeProductRepository
{
    Task<IEnumerable<EmployeeProduct>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(EmployeeProduct employeeProduct, CancellationToken cancellationToken);
    Task DeleteAsync(EmployeeProduct employeeProduct, CancellationToken cancellationToken);
}