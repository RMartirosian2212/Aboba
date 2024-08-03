using Aboba.Application.Interfaces;
using Aboba.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aboba.Infrastucture.Data.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Product?>> GetAllProductsAsync(CancellationToken ct)
    {
        return await _db.Products.ToListAsync(ct);
    }

    public async Task<Product?> GetAsync(int? id, CancellationToken ct)
    {
        return await _db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Product?> GetProductByNameAsync(string name, CancellationToken ct)
    {
        return await _db.Products.FirstOrDefaultAsync(x => x.Name == name, cancellationToken: ct);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken ct)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);
        return product;
    }

    public async Task UpdateAsync(Product product, CancellationToken ct)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Product product, CancellationToken ct)
    {
        var productToDelete = await _db.Products.FindAsync(product.Id, ct);
        if (productToDelete != null)
        {
            _db.Products.Remove(productToDelete);
            await _db.SaveChangesAsync(ct);
        }
    }
}