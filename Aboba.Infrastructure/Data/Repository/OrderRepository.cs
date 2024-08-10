using Aboba.Application.Interfaces;
using Aboba.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aboba.Infrastucture.Data.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _db;

    public OrderRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync(CancellationToken ct)
    {
        return await _db.Orders.ToListAsync(ct);
    }

    public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct)
    {
        return await _db.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .Include(o => o.OrderProducts)
            .ThenInclude(oe => oe.Employee)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        return await _db.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Employee)
            .Where(o => o.UploadDate >= startDate && o.UploadDate <= endDate)
            .ToListAsync(ct);
    }

    public async Task<List<Order>> GetLastMonthOrdersAsync(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        return await _db.Orders
            .Where(o => o.UploadDate >= startDate && o.UploadDate <= endDate)
            .ToListAsync(ct);
    }

    public async Task<Order> AddOrderAsync(Order order, CancellationToken ct)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);
        return order;
    }

    public async Task<Order> UpdateOrderAsync(Order order, CancellationToken ct)
    {
        _db.Orders.Update(order);
        await _db.SaveChangesAsync(ct);
        return order;
    }

    public async Task DeleteOrderAsync(Order order, CancellationToken ct)
    {
        {
            var orderToDelete = await _db.Orders.FirstOrDefaultAsync(x => x.Id == order.Id, ct);
            if (orderToDelete != null)
            {
                _db.Orders.Remove(orderToDelete);
                await _db.SaveChangesAsync(ct);
            }
        }
    }

    public async Task DeleteLastMonthOrdersAsync(DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        var ordersToDelete = await GetOrdersByDateRangeAsync(startDate, endDate, ct);
        _db.Orders.RemoveRange(ordersToDelete);
        await _db.SaveChangesAsync(ct);
    }
}