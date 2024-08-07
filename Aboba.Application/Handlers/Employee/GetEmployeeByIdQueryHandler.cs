using Aboba.Application.Interfaces;
using Aboba.Application.Queries.Employee;
using MediatR;

namespace Aboba.Application.Handlers.Employee;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<Domain.Entities.Employee>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public async Task<Result<Domain.Entities.Employee>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (employee is null)
        {
            return Result<Domain.Entities.Employee>.FromError("No employee with that Id");
        }

        return Result<Domain.Entities.Employee>.FromSuccess(employee);
    }
}