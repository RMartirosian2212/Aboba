using Aboba.Domain.Entities;

namespace Aboba.Application.Services;

public interface IEmployeeSalaryCalculator
{
    Task CalculateEmployeeSalary(List<OrderProduct> orderProducts, CancellationToken cancellationToken);
    Task RecalculateSalaryOnProductPriceChange(int productId, decimal newPrice, CancellationToken cancellationToken);

}