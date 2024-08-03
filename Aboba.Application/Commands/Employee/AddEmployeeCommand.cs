using MediatR;

namespace Aboba.Application.Commands.Employee;

public record AddEmployeeCommand(Domain.Entities.Employee Employee) : IRequest<Result>;