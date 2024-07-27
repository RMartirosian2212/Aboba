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
        var product = new Domain.Entities.Product()
        {
            Name = request.product.Name,
            Price = request.product.Price,
            CreatedAt = DateTime.UtcNow,
            LastChange = DateTime.UtcNow
        };
        await _productRepository.AddAsync(product, cancellationToken);
        return Result.FromSuccess();
    }
}