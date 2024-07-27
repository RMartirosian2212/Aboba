using MediatR;

namespace Aboba.Application.Queries.Order;

public record GetOrdersQuery : IRequest<Result<IEnumerable<Domain.Entities.Order>>>;