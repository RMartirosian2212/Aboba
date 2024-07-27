using Aboba.Domain.Entities;
using MediatR;

namespace Aboba.Application.Queries.GetProducts;

public record GetProductByIdQuery(int? id) : IRequest<Result<Product>>;