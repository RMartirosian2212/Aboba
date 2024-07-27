using MediatR;

namespace Aboba.Application.Queries.Order;

public record GetOrderByIdQuery(int Id) : IRequest<Result<Domain.Entities.Order>>;