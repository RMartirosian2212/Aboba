using Aboba.Domain.Entities;
using MediatR;

namespace Aboba.Application.Queries.GetProducts;

public record GetProductsQuery : IRequest<Result<IEnumerable<Product>>>;