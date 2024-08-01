using Aboba.Application.Commands.Order;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Order;

public class DeleteLastMonthOrdersCommandHandler : IRequestHandler<DeleteLastMonthOrdersCommand, Result>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteLastMonthOrdersCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Result> Handle(DeleteLastMonthOrdersCommand request, CancellationToken cancellationToken)
    {
        var localTime = TimeHelper.GetCzechLocalTime(DateTime.UtcNow);
        var previousMonth = localTime.AddMonths(-1);
        var startDate = new DateTime(previousMonth.Year, previousMonth.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        await _orderRepository.DeleteLastMonthOrdersAsync(startDate, endDate, cancellationToken);
        return Result.FromSuccess();
    }
}