using Aboba.Application.Commands.Order;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Order;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        await _orderRepository.DeleteOrderAsync(request.Order, cancellationToken);
        return Result.FromSuccess();
    }
}