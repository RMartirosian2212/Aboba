using MediatR;

namespace Aboba.Application.Commands.Order;

public record DeleteLastMonthOrdersCommand() : IRequest<Result>;