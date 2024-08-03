using Aboba.Application.Interfaces;
using Aboba.Application.Queries.Employee;
using MediatR;

namespace Aboba.Application.Handlers.Employee;

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeesQuery, Result<IEnumerable<Domain.Entities.Employee>>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public async Task<Result<IEnumerable<Domain.Entities.Employee>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllAsync(cancellationToken);
        if (employees is null)
        {
            return Result<IEnumerable<Domain.Entities.Employee>>.FromError("Zero employees");
        }
        return Result<IEnumerable<Domain.Entities.Employee>>.FromSuccess(employees);
    }
}