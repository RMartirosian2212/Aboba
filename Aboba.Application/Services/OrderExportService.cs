using Aboba.Domain.Entities;
using OfficeOpenXml;

namespace Aboba.Application.Services;

public class OrderExportService : IOrderExportService
{
    public async Task<byte[]> ExportOrdersToExcelAsync(List<Order> orders)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Orders");

            // Заголовки
            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Title";
            worksheet.Cells[1, 3].Value = "Total Price";
            worksheet.Cells[1, 4].Value = "Upload Date";
            worksheet.Cells[1, 5].Value = "Last Change";
            worksheet.Cells[1, 6].Value = "Product Name";
            worksheet.Cells[1, 7].Value = "Quantity";

            // Данные
            int row = 2;
            foreach (var order in orders)
            {
                foreach (var orderProduct in order.OrderProducts)
                {
                    worksheet.Cells[row, 1].Value = order.Id;
                    worksheet.Cells[row, 2].Value = order.Title;
                    worksheet.Cells[row, 3].Value = order.TotalPrice;
                    worksheet.Cells[row, 4].Value = order.UploadDate.ToString("dd.MM.yyyy HH:mm:ss");
                    worksheet.Cells[row, 5].Value = order.LastChange.ToString("dd.MM.yyyy HH:mm:ss");
                    worksheet.Cells[row, 6].Value = orderProduct.Product.Name;
                    worksheet.Cells[row, 7].Value = orderProduct.Quantity;
                    row++;
                }
            }

            return await Task.FromResult(await package.GetAsByteArrayAsync());
        }
    }
}