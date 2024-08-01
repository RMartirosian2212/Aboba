using Aboba.Application.Commands.Order;
using Aboba.Application.Commands.OrderProduct;
using Aboba.Application.Interfaces;
using Aboba.Application.Queries.Order;
using Aboba.Application.Services;
using Aboba.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aboba.Controllers;

public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IOrderService _orderService;
    private readonly IMediator _mediator;
    private readonly IExcelOrderProductProcessor _excelOrderProductProcessor;
    private readonly IOrderExportService _orderExportService;

    public OrderController(IOrderRepository orderRepository, IOrderService orderService,
        IProductRepository productRepository, IOrderProductRepository orderProductRepository, IMediator mediator,
        IExcelOrderProductProcessor excelOrderProductProcessor, IOrderExportService orderExportService)
    {
        _orderRepository = orderRepository;
        _orderService = orderService;
        _productRepository = productRepository;
        _orderProductRepository = orderProductRepository;
        _mediator = mediator;
        _excelOrderProductProcessor = excelOrderProductProcessor;
        _orderExportService = orderExportService;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var orders = await _mediator.Send(new GetOrdersQuery(), ct);
        return View(orders.Value);
    }

    [HttpGet]
    public IActionResult Create(CancellationToken ct)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("File", "Please select a file.");
            return View("Index");
        }

        var orderProducts = await _excelOrderProductProcessor.ProcessExcelFileAsync(file, ct);
        decimal totalPrice = await _orderService.CalculateTotalPriceAsync(orderProducts, ct);

        ViewBag.FileName = file.FileName.Replace(".xlsx", "");
        ViewBag.TotalPrice = totalPrice;

        return View(nameof(Create), orderProducts);
    }

    [HttpPost]
    public async Task<IActionResult> SaveToDb(string orderTitle, decimal totalPrice, List<OrderProduct> orderProducts,
        CancellationToken ct)
    {
        if (string.IsNullOrEmpty(orderTitle))
        {
            ModelState.AddModelError("OrderTitle", "Order title is required.");
            return View(nameof(Create), orderProducts);
        }

        var order = await _mediator.Send(new AddOrderCommand(orderTitle, totalPrice), ct);
        await _mediator.Send(new AddOrderProductCommand(orderProducts, order.Value.Id), ct);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Review(int id, CancellationToken ct)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id), ct);
        return View(order.Value);
    }

    [HttpPost, ActionName("Review")]
    public async Task<IActionResult> ReviewPost(int orderId, CancellationToken ct)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId, ct);
        if (order == null)
        {
            return NotFound();
        }

        decimal totalPrice = await _orderService.CalculateTotalPriceAsync(order.OrderProducts.ToList(), ct);
        var result = await _mediator.Send(new UpdateOrderPriceCommand(order, totalPrice));

        return View(order);
    }

    [HttpGet]
    public async Task<IActionResult> LoadDeleteConfirmation(int id, CancellationToken ct)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id), ct);

        if (order == null)
        {
            return NotFound();
        }

        return PartialView("_OrderDeleteConfirmation", order.Value);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteOrder(int id, CancellationToken ct)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id), ct);
        if (order is null)
        {
            return BadRequest();
        }

        await _mediator.Send(new DeleteOrderCommand(order.Value), ct);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Export()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Export(string? fileName, DateTime startDate, DateTime endDate,
        CancellationToken ct)
    {
        var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate, ct);
        var fileContent = await _orderExportService.ExportOrdersToExcelAsync(startDate, endDate, fileName, orders);
        var fileDownloadName = fileName ?? $"order {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";

        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileDownloadName}.xlsx");
    }
}