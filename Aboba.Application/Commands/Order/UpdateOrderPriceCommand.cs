using MediatR;

namespace Aboba.Application.Commands.Order;

public record UpdateOrderPriceCommand (Domain.Entities.Order Order, decimal TotalPrice, DateTime LastChange) : IRequest<Result<Domain.Entities.Order>>;