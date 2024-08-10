using Aboba.Application.DTOs;
using MediatR;

namespace Aboba.Application.Queries.GetProducts;

public record GetProductsByEmployeeIdQuery(int Id) : IRequest<IEnumerable<OrderProductDto>>;