using MediatR;

namespace Aboba.Application.Commands.Order;

public record AddOrderCommand(string OrderTitle, decimal TotalPrice) : IRequest<Result<Domain.Entities.Order>>;