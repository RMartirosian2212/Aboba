﻿using Aboba.Application.Commands.Order;
using Aboba.Application.Commands.OrderProduct;
using Aboba.Application.Interfaces;
using Aboba.Application.Queries.Employee;
using Aboba.Application.Queries.GetProducts;
using Aboba.Application.Queries.Order;
using Aboba.Application.Services;
using Aboba.Domain.Entities;
using Aboba.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aboba.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IOrderService _orderService;
    private readonly IMediator _mediator;
    private readonly IExcelOrderProductProcessor _excelOrderProductProcessor;
    private readonly IOrderExportService _orderExportService;
    private readonly IEmployeeSalaryCalculator _employeeSalaryCalculator;

    public OrderController(IOrderRepository orderRepository, IOrderService orderService,
        IProductRepository productRepository, IOrderProductRepository orderProductRepository, IMediator mediator,
        IExcelOrderProductProcessor excelOrderProductProcessor, IOrderExportService orderExportService,
        IEmployeeSalaryCalculator employeeSalaryCalculator, IEmployeeRepository employeeRepository)
    {
        _orderRepository = orderRepository;
        _orderService = orderService;
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
        _mediator = mediator;
        _excelOrderProductProcessor = excelOrderProductProcessor;
        _orderExportService = orderExportService;
        _employeeSalaryCalculator = employeeSalaryCalculator;
        _employeeRepository = employeeRepository;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(new GetOrdersQuery(), cancellationToken);
        return View(orders.Value);
    }

    [HttpGet]
    public IActionResult Create(CancellationToken cancellationToken)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("File", "Please select a file.");
            return View("Index");
        }

        var orderProducts = await _excelOrderProductProcessor.ProcessExcelFileAsync(file, cancellationToken);
        decimal totalPrice = await _orderService.CalculateTotalPriceAsync(orderProducts, cancellationToken);

        var employees = await _mediator.Send(new GetEmployeesQuery(), cancellationToken);

        var employeeSelectList = employees.Value.Select(e => new SelectListItem
        {
            Value = e.Id.ToString(),
            Text = e.Name
        }).ToList();

        var viewModel = new OrderViewModel
        {
            OrderProducts = orderProducts,
            Employees = employeeSelectList
        };

        ViewBag.FileName = file.FileName.Replace(".xlsx", "");
        ViewBag.TotalPrice = totalPrice;

        return View(nameof(Create), viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> SaveOrderToDb(string orderTitle, decimal totalPrice,
        List<OrderProduct> orderProducts,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(orderTitle))
        {
            var employees = await _mediator.Send(new GetEmployeesQuery(), cancellationToken);

            var employeeSelectList = employees.Value.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }).ToList();

            var viewModel = new OrderViewModel
            {
                OrderProducts = orderProducts,
                Employees = employeeSelectList
            };
            ModelState.AddModelError("OrderTitle", "Order title is required.");
            return View(nameof(Create), viewModel);
        }

        var order = await _mediator.Send(new AddOrderCommand(orderTitle, totalPrice), cancellationToken);
        await _mediator.Send(new AddOrderProductCommand(orderProducts, order.Value.Id), cancellationToken);
        await _employeeSalaryCalculator.CalculateEmployeeSalary(orderProducts, cancellationToken);
        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<IActionResult> Review(int id, CancellationToken cancellationToken)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        if (order == null)
        {
            return NotFound();
        }

        return View(order.Value);
    }
    
    [HttpPost, ActionName("Review")]
    public async Task<IActionResult> ReviewPost(int orderId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken);
        if (order == null)
        {
            return NotFound();
        }

        // Recalculating the total cost of the order
        decimal totalPrice = await _orderService.CalculateTotalPriceAsync(order.OrderProducts.ToList(), cancellationToken);
        await _mediator.Send(new UpdateOrderPriceCommand(order, totalPrice), cancellationToken);
        
        var productIds = order.OrderProducts
            .Select(op => op.Product.Id)
            .Distinct()
            .ToList();
        
        foreach (var productId in productIds)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product != null)
            {
                await _employeeSalaryCalculator.RecalculateSalaryOnProductPriceChange(productId, product.Price, cancellationToken);
            }
        }

        return RedirectToAction(nameof(Review), new { id = orderId });
    }

    [HttpGet]
    public async Task<IActionResult> LoadDeleteConfirmation(int id, CancellationToken cancellationToken)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);

        if (order == null)
        {
            return NotFound();
        }

        return PartialView("_OrderDeleteConfirmation", order.Value);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        if (order is null)
        {
            return BadRequest();
        }

        await _employeeSalaryCalculator.RecalculateSalariesForOrderDeletion(id, cancellationToken);

        await _mediator.Send(new DeleteOrderCommand(order.Value), cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Export()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Export(string? fileName, DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate, cancellationToken);
        var fileContent = await _orderExportService.ExportOrdersToExcelAsync(startDate, endDate, fileName, orders);
        var fileDownloadName = fileName ?? $"order {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";

        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"{fileDownloadName}.xlsx");
    }

    [HttpGet]
    public async Task<IActionResult> OrdersFromPreviousMonth(CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(new GetLastMonthOrdersQuery(), cancellationToken);
        return View(orders.Value);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteOrdersFromPreviousMonth(CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(new DeleteLastMonthOrdersCommand(), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}