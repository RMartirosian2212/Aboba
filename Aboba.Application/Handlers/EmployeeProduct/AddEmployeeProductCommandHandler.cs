using Aboba.Application.Commands.EmployeeProduct;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.EmployeeProduct;

public class AddEmployeeProductCommandHandler : IRequestHandler<AddEmployeeProductCommand, Result>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeProductRepository _employeeProductRepository;

    public AddEmployeeProductCommandHandler(IEmployeeRepository employeeRepository, IEmployeeProductRepository employeeProductRepository)
    {
        _employeeRepository = employeeRepository;
        _employeeProductRepository = employeeProductRepository;
    }

    public async Task<Result> Handle(AddEmployeeProductCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
        {
            return Result.FromError("Employee not found");
        }

        // Проверяем, существует ли уже связь между сотрудником и продуктом
        var existingEmployeeProduct = await _employeeProductRepository.GetByEmployeeIdAndProductIdAsync(request.EmployeeId, request.ProductId, cancellationToken);
        if (existingEmployeeProduct == null)
        {
            var employeeProduct = new Domain.Entities.EmployeeProduct
            {
                EmployeeId = request.EmployeeId,
                ProductId = request.ProductId
            };
            await _employeeProductRepository.AddAsync(employeeProduct, cancellationToken);
        }

        // Обновляем зарплату сотрудника
        employee.Salary += request.ProductPrice;
        await _employeeRepository.UpdateAsync(employee, cancellationToken);

        return Result.FromSuccess();
    }
}