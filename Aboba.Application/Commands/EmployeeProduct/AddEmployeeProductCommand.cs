using MediatR;

namespace Aboba.Application.Commands.EmployeeProduct;

public record AddEmployeeProductCommand(int EmployeeId, int ProductId, decimal ProductPrice) : IRequest<Result>;