using Aboba.Application.Interfaces;
using Aboba.Application.Queries.GetProducts;
using MediatR;

namespace Aboba.Application.Handlers.Product;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<Domain.Entities.Product>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Domain.Entities.Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAsync(request.id, cancellationToken);
        if (product == null)
        {
            return new Error("Product with this Id was not found");
        }
        return product;
    }
}