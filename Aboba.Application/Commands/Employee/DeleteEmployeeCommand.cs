using MediatR;

namespace Aboba.Application.Commands.Employee;

public record DeleteEmployeeCommand(int Id) : IRequest<Result>;