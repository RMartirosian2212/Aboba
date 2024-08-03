using Aboba.Application.Commands.Employee;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.Employee;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result>
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (employee is null)
        {
            return Result.FromError("No employee with that Id");
        }

        await _employeeRepository.DeleteAsync(request.Id, cancellationToken);
        return Result.FromSuccess();
    }
}