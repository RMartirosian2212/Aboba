using Aboba.Application.Commands.OrderProduct;
using Aboba.Application.Interfaces;
using MediatR;

namespace Aboba.Application.Handlers.OrderProduct;

public class AddOrderProductCommandHandler : IRequestHandler<AddOrderProductCommand, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public AddOrderProductCommandHandler(IProductRepository productRepository,
        IOrderProductRepository orderProductRepository, IEmployeeRepository employeeRepository)
    {
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result> Handle(AddOrderProductCommand request, CancellationToken cancellationToken)
    {
        var localTime = TimeHelper.GetCzechLocalTime(DateTime.UtcNow);

        foreach (var orderProduct in request.OrderProducts)
        {
            if (!string.IsNullOrEmpty(orderProduct.ProductName))
            {
                var product = await _productRepository.GetProductByNameAsync(orderProduct.ProductName, cancellationToken);

                if (product == null)
                {
                    product = new Domain.Entities.Product
                    {
                        Name = orderProduct.ProductName,
                        Price = 0, // Установить цену 0 для новых продуктов
                        CreatedAt = localTime,
                        LastChange = localTime
                    };
                    await _productRepository.AddAsync(product, cancellationToken); // Сохранение нового продукта в базе данных
                }

                Domain.Entities.Employee? employee = null;
                if (orderProduct.EmployeeId.HasValue && orderProduct.EmployeeId.Value != 0)
                {
                    employee = await _employeeRepository.GetByIdAsync(orderProduct.EmployeeId.Value, cancellationToken);
                }

                orderProduct.ProductId = product.Id;
                orderProduct.Product = product;
                orderProduct.OrderId = request.OrderId;
                orderProduct.IsInDb = true;
                orderProduct.EmployeeId = employee?.Id;
                orderProduct.Employee = employee;
                await _orderProductRepository.AddOrderProductAsync(orderProduct, cancellationToken);
            }
        }

        return Result.FromSuccess();
    }
}