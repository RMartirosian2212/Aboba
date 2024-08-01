using Aboba.Application.Commands.OrderProduct;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.OrderProduct;

public class AddOrderProductCommandHandler : IRequestHandler<AddOrderProductCommand, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;

    public AddOrderProductCommandHandler(IProductRepository productRepository, IOrderProductRepository orderProductRepository)
    {
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
    }

    public async Task<Result> Handle(AddOrderProductCommand request, CancellationToken cancellationToken)
    {
        var localTime = TimeHelper.GetCzechLocalTime(DateTime.UtcNow);

        foreach (var orderProduct in request.OrderProducts)
        {
            if (!string.IsNullOrEmpty(orderProduct.ProductName))
            {
                var product = await _productRepository.GetProductByNameAsync(orderProduct.ProductName, cancellationToken);
                if (product == null)
                {
                    product = new Domain.Entities.Product
                    {
                        Name = orderProduct.ProductName,
                        Price = 0, // Setting the price to 0 for new products
                        CreatedAt = localTime,
                        LastChange = localTime
                    };
                    await _productRepository.AddAsync(product, cancellationToken); // Saving a new product in the database
                }

                orderProduct.ProductId = product.Id;
                orderProduct.Product = product;
                orderProduct.OrderId = request.OrderId;
                orderProduct.IsInDb = true;
                await _orderProductRepository.AddOrderProductAsync(orderProduct, cancellationToken);
            }
        }

        return Result.FromSuccess();
    }
}