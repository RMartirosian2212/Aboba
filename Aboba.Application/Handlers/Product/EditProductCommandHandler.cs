﻿using Aboba.Application.Commands.Product;
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
        var localTime = TimeHelper.GetCzechLocalTime(DateTime.UtcNow);

        var product = new Domain.Entities.Product()
        {
            Id = request.Product.Id,
            Name = request.Product.Name,
            Price = request.Product.Price,
            CreatedAt = request.Product.CreatedAt,
            LastChange = localTime
        };
        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.FromSuccess();
    }
}