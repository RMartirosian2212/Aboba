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
        var order = new Domain.Entities.Order
        {
            Title = request.OrderTitle,
            TotalPrice = request.TotalPrice,
            UploadDate = DateTime.UtcNow,
            LastChange = DateTime.UtcNow,
            OrderProducts = new List<Domain.Entities.OrderProduct>()
        };
        
        await _orderRepository.AddOrderAsync(order, cancellationToken);
        return Result<Domain.Entities.Order>.FromSuccess(order);
    }
}