using MediatR;

namespace Aboba.Application.Queries.Order;

public record GetLastMonthOrdersQuery() : IRequest<Result<IEnumerable<Domain.Entities.Order>>>;