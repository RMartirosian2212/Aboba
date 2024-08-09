using Aboba.Application.Commands.Employee;
using Aboba.Application.Interfaces;
using Aboba.Application.Queries.Employee;
using Aboba.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aboba.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly IMediator _mediator;
    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var employees = await _mediator.Send(new GetEmployeesQuery(), cancellationToken);
        return View(employees.Value);
    }
    
    [HttpGet]
    public IActionResult Create(CancellationToken cancellationToken)
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Employee employee, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddEmployeeCommand(employee), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);
        return View(employee.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Employee employee, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateEmployeeCommand(employee), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);
        return View(employee.Value);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeletePost(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}