using Aboba.Application.DTOs;
using Aboba.Application.Interfaces;
using Aboba.Application.Queries.GetProducts;
using MediatR;

namespace Aboba.Application.Handlers.Product;

public class GetProductsByEmployeeIdQueryHandler : IRequestHandler<GetProductsByEmployeeIdQuery, IEnumerable<OrderProductDto>>
{
    private readonly IOrderProductRepository _orderProduct;

    public GetProductsByEmployeeIdQueryHandler(IOrderProductRepository orderProduct)
    {
        _orderProduct = orderProduct;
    }
        
    public async Task<IEnumerable<OrderProductDto>> Handle(GetProductsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        return await _orderProduct.GetProductsByEmployeeIdAsync(request.Id, cancellationToken);
    }
}