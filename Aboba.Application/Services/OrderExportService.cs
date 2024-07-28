using Aboba.Domain.Entities;
using OfficeOpenXml;

namespace Aboba.Application.Services;

public class OrderExportService : IOrderExportService
{
    public async Task<byte[]> ExportOrdersToExcelAsync(DateTime startDate, DateTime endDate, string? fileName,
        List<Order> orders)
    {
        // Лицензия на использование EPPlus в некоммерческих целях
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        // Словарь для хранения продуктов и их количества
        var productDictionary = new Dictionary<string, (Product product, int quantity, decimal totalPrice)>();

        decimal overallTotalPrice = 0;

        // Объединение продуктов из заказов
        foreach (var order in orders)
        {
            foreach (var orderProduct in order.OrderProducts)
            {
                if (productDictionary.ContainsKey(orderProduct.Product.Name))
                {
                    productDictionary[orderProduct.Product.Name] = (
                        productDictionary[orderProduct.Product.Name].product,
                        productDictionary[orderProduct.Product.Name].quantity + orderProduct.Quantity,
                        productDictionary[orderProduct.Product.Name].totalPrice +
                        (orderProduct.Quantity * orderProduct.Product.Price)
                    );
                }
                else
                {
                    productDictionary[orderProduct.Product.Name] = (
                        orderProduct.Product,
                        orderProduct.Quantity,
                        orderProduct.Quantity * orderProduct.Product.Price
                    );
                }
            }

            overallTotalPrice += order.TotalPrice;
        }

        // Создание Excel файла
        using (var package = new ExcelPackage())
        {
            string sheetName = fileName ?? $"order {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            // Заголовки
            worksheet.Cells[1, 1].Value = "Product Id";
            worksheet.Cells[1, 2].Value = "Product Name";
            worksheet.Cells[1, 3].Value = "Quantity";
            worksheet.Cells[1, 4].Value = "Total Price";

            worksheet.Column(1).Width = 15; // Ширина для Product Id
            worksheet.Column(2).Width = 35; // Ширина для Product Name
            worksheet.Column(3).Width = 20; // Ширина для Quantity
            worksheet.Column(4).Width = 15; // Ширина для Total Price


            // Данные
            int row = 2;
            foreach (var kvp in productDictionary)
            {
                worksheet.Cells[row, 1].Value = kvp.Value.product.Id;
                worksheet.Cells[row, 2].Value = kvp.Value.product.Name;
                worksheet.Cells[row, 3].Value = kvp.Value.quantity;
                worksheet.Cells[row, 4].Value = kvp.Value.totalPrice;
                row++;
            }

            // Общая сумма всех заказов
            worksheet.Cells[row, 3].Value = "Overall Total Price:";
            worksheet.Cells[row, 4].Value = overallTotalPrice;

            return await package.GetAsByteArrayAsync();
        }
    }
}