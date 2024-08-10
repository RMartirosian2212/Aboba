using Aboba.Application.DTOs;
using Aboba.Domain.Entities;
using Aboba.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aboba.Infrastucture.Data.Repository;

public class OrderProductRepository : IOrderProductRepository
{
    private readonly ApplicationDbContext _db;

    public OrderProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<OrderProduct>> GetAllOrderProductAsync(CancellationToken ct)
    {
        return await _db.OrderProducts.Include(x => x.Order).Include(x => x.Product).ToListAsync(ct);
    }

    public async Task<OrderProduct> AddOrderProductAsync(OrderProduct orderProduct, CancellationToken ct)
    {
        _db.OrderProducts.Add(orderProduct);
        await _db.SaveChangesAsync(ct);
        return orderProduct;
    }
    public async Task<IEnumerable<OrderProduct>> GetOrderProductsByProductId(int productId, CancellationToken cancellationToken)
    {
        return await _db.OrderProducts
            .Where(op => op.ProductId == productId)
            .Include(op => op.Product)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderProductDto>> GetProductsByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
    {
        return await _db.OrderProducts
            .Where(op => op.EmployeeId == employeeId)
            .Select(op => new OrderProductDto
            {
                ProductId = op.ProductId,
                ProductName = op.Product.Name!,
                OrderId = op.OrderId,
                OrderTitle = op.Order.Title,
                Quantity = op.Quantity,
                Price = op.Product.Price
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderProduct>> GetOrderProductsByOrderId(int orderId, CancellationToken cancellationToken)
    {
        return await _db.OrderProducts
            .Where(op => op.OrderId == orderId)
            .Include(op => op.Product)
            .ToListAsync(cancellationToken);
    }


    public async Task DeleteOrderProductAsync(OrderProduct orderProduct, CancellationToken ct)
    {
        _db.OrderProducts.Remove(orderProduct);
        await _db.SaveChangesAsync(ct);
    }
}
