using MediatR;

namespace Aboba.Application.Queries.Employee;

public record GetEmployeesQuery() : IRequest<Result<IEnumerable<Domain.Entities.Employee>>>;