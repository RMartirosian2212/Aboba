using System.Collections;
using Aboba.Application.Interfaces;
using Aboba.Application.Queries.GetProducts;
using Aboba.Domain.Entities;
using MediatR;

namespace Aboba.Application.Handlers.Product;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<Domain.Entities.Product>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Result<IEnumerable<Domain.Entities.Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllProductsAsync(cancellationToken);
        if (products == null)
        {
            return new Error("No products were found");
        }

        return Result<IEnumerable<Domain.Entities.Product>>.FromSuccess(products);
    }
}