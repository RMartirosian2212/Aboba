using Aboba.Application.Interfaces;
using Aboba.Application.Queries.Order;
using MediatR;

namespace Aboba.Application.Handlers.Order;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Result<IEnumerable<Domain.Entities.Order>>>
{
    private readonly IOrderRepository? _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository? orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<IEnumerable<Domain.Entities.Order>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository?.GetAllOrdersAsync(cancellationToken);
        return Result<IEnumerable<Domain.Entities.Order>>.FromSuccess(orders);
    }
}