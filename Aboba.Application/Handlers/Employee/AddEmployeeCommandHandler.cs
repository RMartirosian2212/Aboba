using Aboba.Application.Commands.Employee;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Employee;

public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, Result>
{
    private readonly IEmployeeRepository _employeeRepository;

    public AddEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public async Task<Result> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        await _employeeRepository.AddAsync(request.Employee, cancellationToken);
        return Result.FromSuccess();
    }
}