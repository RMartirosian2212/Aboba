using Aboba.Application.Commands.Product;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Product;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _productRepository.DeleteAsync(request.Product, cancellationToken);
        return Result.FromSuccess();
    }
}