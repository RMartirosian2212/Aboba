using Aboba.Application.Interfaces;
using Aboba.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aboba.Infrastucture.Data.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _db;

    public EmployeeRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _db.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Employee?>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Employees.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Employee employee, CancellationToken cancellationToken)
    {
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken)
    {
        _db.Entry(employee).State = EntityState.Modified;
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var employee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (employee != null)
        {
            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}