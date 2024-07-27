using Aboba.Application.Interfaces;
using Aboba.Application.Services;
using Aboba.Domain.Entities;

namespace Aboba.Services;

public class OrderService : IOrderService
{
    private readonly IProductRepository _productRepository;

    public OrderService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<decimal> CalculateTotalPriceAsync(List<OrderProduct> orderProducts, CancellationToken ct)
    {
        decimal totalPrice = 0;

        foreach (var item in orderProducts)
        {
            if (item.Product != null && item.Product.Id != 0)
            {
                item.IsInDb = true;
                totalPrice += item.Product.Price * item.Quantity;
            }
            else
            {
                item.IsInDb = false;
            }
        }

        return totalPrice;
    }

}