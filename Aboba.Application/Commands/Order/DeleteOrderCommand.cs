using MediatR;

namespace Aboba.Application.Commands.Order;

public record DeleteOrderCommand(Domain.Entities.Order Order) : IRequest<Result>;