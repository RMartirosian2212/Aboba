using Aboba.Application.Commands.Product;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Product;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result>
{
    private readonly IProductRepository _productRepository;

    public AddProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Result> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var localTime = TimeHelper.GetCzechLocalTime(DateTime.UtcNow);

        var product = new Domain.Entities.Product()
        {
            Name = request.Product.Name,
            Price = request.Product.Price,
            CreatedAt = localTime,
            LastChange = localTime
        };
        await _productRepository.AddAsync(product, cancellationToken);
        return Result.FromSuccess();
    }
}