using MediatR;

namespace Aboba.Application.Queries.Employee;

public record GetEmployeeByIdQuery(int Id) : IRequest<Result<Domain.Entities.Employee>>;