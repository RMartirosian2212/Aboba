using Aboba.Application.Interfaces;
using Aboba.Application.Queries.Order;
using MediatR;

namespace Aboba.Application.Handlers.Order;

public class GetLastMonthOrdersQueryHandler : IRequestHandler<GetLastMonthOrdersQuery, Result<IEnumerable<Domain.Entities.Order>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetLastMonthOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<IEnumerable<Domain.Entities.Order>>> Handle(GetLastMonthOrdersQuery request, CancellationToken cancellationToken)
    {
        var localTime = TimeHelper.GetCzechLocalTime(DateTime.Now);
        var previousMonth = localTime.AddMonths(-1);
        var startDate = new DateTime(previousMonth.Year, previousMonth.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var orders = await _orderRepository.GetLastMonthOrdersAsync(startDate, endDate, cancellationToken);
        return Result<IEnumerable<Domain.Entities.Order>>.FromSuccess(orders);
    }
}