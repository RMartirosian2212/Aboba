using Aboba.Domain.Entities;

namespace Aboba.Application.Interfaces;

public interface IProductRepository
{
    public Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken ct);
    public Task<Product?> GetByIdAsync(int? id, CancellationToken ct);
    public Task<Product?> GetProductByNameAsync(string name, CancellationToken ct);
    public Task<Product> AddAsync(Product product, CancellationToken ct);
    public Task UpdateAsync(Product product, CancellationToken ct);
    public Task DeleteAsync(Product product, CancellationToken ct);
    

}