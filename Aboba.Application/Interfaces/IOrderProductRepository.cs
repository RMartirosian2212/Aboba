using Aboba.Domain.Entities;

namespace Aboba.Application.Interfaces;

public interface IOrderProductRepository
{
    public Task<IEnumerable<OrderProduct>> GetAllOrderProductAsync(CancellationToken ct);
    public Task<OrderProduct> AddOrderProductAsync(OrderProduct orderProduct, CancellationToken ct);
    public Task DeleteOrderProductAsync(OrderProduct orderProduct, CancellationToken ct);
}