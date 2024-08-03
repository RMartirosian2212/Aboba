using Aboba.Application.Commands.Employee;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Employee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Employee.Id, cancellationToken);
        if (employee is null)
        {
            return Result.FromError("No employee with that Id");
        }

        await _employeeRepository.UpdateAsync(request.Employee, cancellationToken);
        return Result.FromSuccess();
    }
}