using Aboba.Application.Commands.Product;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Product;

public class EditProductCommandHandler : IRequestHandler<EditProductCommand,Result>
{
    private readonly IProductRepository _productRepository;

    public EditProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        
        var product = new Domain.Entities.Product()
        {
            Id = request.product.Id,
            Name = request.product.Name,
            Price = request.product.Price,
            CreatedAt = request.product.CreatedAt,
            LastChange = DateTime.UtcNow
        };
        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.FromSuccess();
    }
}