using Aboba.Application.Interfaces;
using Aboba.Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Aboba.Application.Services;

public class ExcelOrderProductProcessor : IExcelOrderProductProcessor
{
    private readonly IProductRepository _productRepository;

    public ExcelOrderProductProcessor(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

   public async Task<List<OrderProduct>> ProcessExcelFileAsync(IFormFile file, CancellationToken ct)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var orderProducts = new List<OrderProduct>();

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string productName = worksheet.Cells[row, 3].Text;

                    if (!string.IsNullOrEmpty(productName))
                    {
                        var existingOrderProduct = orderProducts
                            .FirstOrDefault(op => op.Product != null && op.Product.Name == productName);

                        if (existingOrderProduct != null)
                        {
                            existingOrderProduct.Quantity++;
                        }
                        else
                        {
                            var product = await _productRepository.GetProductByNameAsync(productName, ct);
                            if (product == null)
                            {
                                product = new Product
                                {
                                    Name = productName,
                                    Price = 0 // Default price for new products not found in the database
                                };
                            }

                            orderProducts.Add(new OrderProduct
                            {
                                Product = product,
                                Quantity = 1,
                                IsInDb = product.Id != 0 // Check if the product has been found in the database
                            });
                        }
                    }
                }
            }
        }

        return orderProducts;
    }
}