using Aboba.Application.Commands.Product;
using Aboba.Application.Interfaces;
using Aboba.Application.Queries.GetProducts;
using Aboba.Application.Services;
using Aboba.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aboba.Controllers;

public class ProductController : Controller
{
    private readonly IMediator _mediator;
    private readonly IEmployeeSalaryCalculator _employeeSalaryCalculator;

    public ProductController(IMediator mediator, IEmployeeSalaryCalculator employeeSalaryCalculator)
    {
        _mediator = mediator;
        _employeeSalaryCalculator = employeeSalaryCalculator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductsQuery(), ct);
        
        return View(result.Value);
    }

    [HttpGet]
    public IActionResult Create(CancellationToken ct)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        await _mediator.Send(new AddProductCommand(product), ct);
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id, CancellationToken ct)
    {
        if (id is 0 or null)
        {
            return NotFound();
        }

        var product = await _mediator.Send(new GetProductByIdQuery(id), ct);
        if (product == null)
        {
            return NotFound();
        }

        return View(product.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Product product, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        var existingProduct = await _mediator.Send(new GetProductByIdQuery(product.Id), cancellationToken);
        if (existingProduct == null)
        {
            return NotFound();
        }

        var salaryCalculator = HttpContext.RequestServices.GetService<IEmployeeSalaryCalculator>();
        if (salaryCalculator != null && existingProduct.Value.Price != product.Price)
        {
            await salaryCalculator.RecalculateSalaryOnProductPriceChange(existingProduct.Value.Id, product.Price, cancellationToken);
        }

        existingProduct.Value.Name = product.Name;
        existingProduct.Value.Price = product.Price;

        await _mediator.Send(new EditProductCommand(existingProduct.Value), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id, CancellationToken ct)
    {
        if (id is 0 or null)
        {
            return NotFound();
        }

        var product = await _mediator.Send(new GetProductByIdQuery(id), ct);
        if (product == null)
        {
            return NotFound();
        }

        return View(product.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Product? product, CancellationToken ct)
    {
        if (product == null)
        {
            return NotFound();
        }

        await _mediator.Send(new DeleteProductCommand(product), ct);

        return RedirectToAction(nameof(Index));
    }
}