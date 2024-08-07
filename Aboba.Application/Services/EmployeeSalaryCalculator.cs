using Aboba.Application.Interfaces;
using Aboba.Domain.Entities;

namespace Aboba.Application.Services;

public class EmployeeSalaryCalculator : IEmployeeSalaryCalculator
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeSalaryCalculator(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public Task CalculateEmployeeSalary(List<OrderProduct> orderProducts)
    {
        foreach (var item in orderProducts)
        {
            if (item.EmployeeId != 0)
            {
                var employee = _employeeRepository.GetByIdAsync(item.EmployeeId)
            }
        }
    }
}