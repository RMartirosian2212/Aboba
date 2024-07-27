using MediatR;

namespace Aboba.Application.Commands.Product;

public record DeleteProductCommand(Domain.Entities.Product Product) : IRequest<Result>;