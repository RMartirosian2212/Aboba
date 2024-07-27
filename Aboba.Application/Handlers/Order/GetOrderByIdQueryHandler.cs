using Aboba.Application.Interfaces;
using Aboba.Application.Queries.Order;
using MediatR;

namespace Aboba.Application.Handlers.Order;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<Domain.Entities.Order>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }


    public async Task<Result<Domain.Entities.Order>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByIdAsync(request.Id, cancellationToken);
        return Result<Domain.Entities.Order>.FromSuccess(order);
    }
}