using Aboba.Domain.Entities;

namespace Aboba.Application.Services;

public interface IOrderExportService
{
    Task<byte[]> ExportOrdersToExcelAsync(DateTime startDate, DateTime endDate, string? fileName, List<Order> orders);
}