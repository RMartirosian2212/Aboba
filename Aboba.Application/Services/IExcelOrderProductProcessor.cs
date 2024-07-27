using Aboba.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Aboba.Application.Services;

public interface IExcelOrderProductProcessor
{
    Task<List<OrderProduct>> ProcessExcelFileAsync(IFormFile fileStream, CancellationToken ct);
}