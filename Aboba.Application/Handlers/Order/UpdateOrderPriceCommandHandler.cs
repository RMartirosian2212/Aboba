using Aboba.Application.Commands.Order;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Order;

public class UpdateOrderPriceCommandHandler : IRequestHandler<UpdateOrderPriceCommand, Result<Domain.Entities.Order>>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderPriceCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<Domain.Entities.Order>> Handle(UpdateOrderPriceCommand request, CancellationToken cancellationToken)
    {
        var localTime = TimeHelper.GetCzechLocalTime(DateTime.UtcNow);
   
        var order = request.Order;
        order.TotalPrice = request.TotalPrice;
        order.LastChange = localTime;
        await _orderRepository.UpdateOrderAsync(order, cancellationToken);
        return Result<Domain.Entities.Order>.FromSuccess(order);
    }
}