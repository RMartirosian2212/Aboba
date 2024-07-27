using Aboba.Domain.Entities;

namespace Aboba.Application.Services;

public interface IOrderService
{
    Task<decimal> CalculateTotalPriceAsync(List<OrderProduct> orderProducts, CancellationToken ct);
}