using MediatR;

namespace Aboba.Application.Commands.Product;

public record AddProductCommand(Domain.Entities.Product product) : IRequest<Result>;