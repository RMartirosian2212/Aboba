using Aboba.Domain.Entities;

namespace Aboba.Application.Services;

public interface IOrderExportService
{
    Task<byte[]> ExportOrdersToExcelAsync(List<Order> orders);
}