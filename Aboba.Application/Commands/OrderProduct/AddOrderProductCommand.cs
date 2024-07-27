using MediatR;

namespace Aboba.Application.Commands.OrderProduct;

public record AddOrderProductCommand(IEnumerable<Domain.Entities.OrderProduct> OrderProducts, int OrderId) : IRequest<Result>;