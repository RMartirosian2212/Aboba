using Aboba.Application.Commands.Order;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Order;

public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Result<Domain.Entities.Order>>
{
    private readonly IOrderRepository _orderRepository;

    public AddOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<Domain.Entities.Order>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        var localTime = TimeHelper.GetCzechLocalTime(DateTime.Now);

        var order = new Domain.Entities.Order
        {
            Title = request.OrderTitle,
            TotalPrice = request.TotalPrice,
            UploadDate = localTime,
            LastChange = localTime,
            OrderProducts = new List<Domain.Entities.OrderProduct>()
        };
        
        await _orderRepository.AddOrderAsync(order, cancellationToken);
        return Result<Domain.Entities.Order>.FromSuccess(order);
    }
}