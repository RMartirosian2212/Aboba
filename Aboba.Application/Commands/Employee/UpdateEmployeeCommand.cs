using MediatR;

namespace Aboba.Application.Commands.Employee;

public record UpdateEmployeeCommand(Domain.Entities.Employee Employee) : IRequest<Result>;