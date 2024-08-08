using Aboba.Application.Interfaces;
using Aboba.Domain.Entities;

namespace Aboba.Application.Services;

public class EmployeeSalaryCalculator : IEmployeeSalaryCalculator
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;

    public EmployeeSalaryCalculator(IEmployeeRepository employeeRepository, IProductRepository productRepository, IOrderProductRepository orderProductRepository)
    {
        _employeeRepository = employeeRepository;
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
    }

    public async Task CalculateEmployeeSalary(List<OrderProduct> orderProducts, CancellationToken cancellationToken)
    {
        foreach (var orderProduct in orderProducts)
        {
            if (orderProduct.EmployeeId.HasValue && orderProduct.EmployeeId.Value != 0)
            {
                var employee = await _employeeRepository.GetByIdAsync(orderProduct.EmployeeId.Value, cancellationToken);
                if (employee != null)
                {
                    var product = await _productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);
                    if (product != null)
                    {
                        employee.Salary += product.Price * orderProduct.Quantity;
                        await _employeeRepository.UpdateAsync(employee, cancellationToken);
                    }
                }
            }
        }
    }
    public async Task RecalculateSalaryOnProductPriceChange(int productId, decimal newPrice, CancellationToken cancellationToken)
    {
        var orderProducts = await _orderProductRepository.GetOrderProductsByProductId(productId, cancellationToken);
        foreach (var orderProduct in orderProducts)
        {
            if (orderProduct.EmployeeId.HasValue && orderProduct.EmployeeId.Value != 0)
            {
                var employee = await _employeeRepository.GetByIdAsync(orderProduct.EmployeeId.Value, cancellationToken);
                if (employee != null)
                {
                    employee.Salary -= orderProduct.Product.Price * orderProduct.Quantity;
                    employee.Salary += newPrice * orderProduct.Quantity;
                    await _employeeRepository.UpdateAsync(employee, cancellationToken);
                }
            }
        }
    }
}