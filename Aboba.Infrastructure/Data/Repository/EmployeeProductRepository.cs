using Aboba.Application.Interfaces;
using Aboba.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aboba.Infrastucture.Data.Repository;

public class EmployeeProductRepository : IEmployeeProductRepository
{
    private readonly ApplicationDbContext _db;

    public EmployeeProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<EmployeeProduct>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.EmployeeProducts
            .Include(ep => ep.Employee)
            .Include(ep => ep.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeProduct?> GetByEmployeeIdAndProductIdAsync(int employeeId, int productId, CancellationToken cancellationToken)
    {
        return await _db.EmployeeProducts
            .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProductId == productId, cancellationToken);
    }

    public async Task AddAsync(EmployeeProduct employeeProduct, CancellationToken cancellationToken)
    {
        _db.EmployeeProducts.Add(employeeProduct);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(EmployeeProduct employeeProduct, CancellationToken cancellationToken)
    {
        _db.EmployeeProducts.Remove(employeeProduct);
        await _db.SaveChangesAsync(cancellationToken);
    }
}