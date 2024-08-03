using MediatR;

namespace Aboba.Application.Queries.Employee;

public record GetEmployeeByIdQuery(int id) : IRequest<Result<Domain.Entities.Employee>>;