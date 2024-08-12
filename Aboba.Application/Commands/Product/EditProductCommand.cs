using MediatR;

namespace Aboba.Application.Commands.Product;

public record EditProductCommand(Domain.Entities.Product Product) : IRequest<Result>;