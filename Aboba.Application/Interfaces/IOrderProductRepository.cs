using Aboba.Domain.Entities;

namespace Aboba.Application.Interfaces;

public interface IOrderProductRepository
{
    public Task<IEnumerable<OrderProduct>> GetAllOrderProductAsync(CancellationToken ct);
    public Task<OrderProduct> AddOrderProductAsync(OrderProduct orderProduct, CancellationToken ct);
    Task<IEnumerable<OrderProduct>> GetOrderProductsByProductId(int productId, CancellationToken cancellationToken);

    public Task<IEnumerable<OrderProduct>> GetOrderProductsByOrderId(int orderId, CancellationToken cancellationToken);

    public Task DeleteOrderProductAsync(OrderProduct orderProduct, CancellationToken ct);
}