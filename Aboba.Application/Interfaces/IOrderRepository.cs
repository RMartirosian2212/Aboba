using Aboba.Domain.Entities;

namespace Aboba.Application.Interfaces;

public interface IOrderRepository
{
    public Task<IEnumerable<Order>> GetAllOrdersAsync(CancellationToken ct);
    public Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct);
    public Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct);
    public Task<List<Order>> GetLastMonthOrdersAsync(DateTime startDate, DateTime endDate, CancellationToken ct);
    public Task<Order> AddOrderAsync(Order order, CancellationToken ct);
    public Task<Order> UpdateOrderAsync(Order order, CancellationToken ct);
    public Task DeleteOrderAsync(Order order, CancellationToken ct);
    public Task DeleteLastMonthOrdersAsync(DateTime startDate, DateTime endDate, CancellationToken ct);
}