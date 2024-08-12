using Aboba.Domain.Entities;
using OfficeOpenXml;

namespace Aboba.Application.Services
{
    public class OrderExportService : IOrderExportService
    {
        public async Task<byte[]> ExportOrdersToExcelAsync(DateTime startDate, DateTime endDate, string? fileName, List<Order> orders)
        {
            // License for non-commercial use of EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Vocabulary for food storage and quantities
            var productDictionary = new Dictionary<string, (Product product, int quantity, decimal totalPrice)>();

            // List for storing employee information
            var employeeProductList = new List<(Employee employee, Product product, Order order, decimal price, int quantity, decimal totalProductPrice)>();

            decimal overallTotalPrice = 0;

            // Combining products from orders and preparing data for employees
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

                    // Filling in the data for the second sheet
                    if (orderProduct.Employee != null)
                    {
                        employeeProductList.Add((
                            orderProduct.Employee,
                            orderProduct.Product,
                            orderProduct.Order,
                            orderProduct.Product.Price,
                            orderProduct.Quantity,
                            orderProduct.Quantity * orderProduct.Product.Price
                        ));
                    }
                }

                overallTotalPrice += order.TotalPrice;
            }

            // Creating an Excel file
            using (var package = new ExcelPackage())
            {
                // First sheet
                string sheetName1 = fileName ?? $"order {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";
                var worksheet1 = package.Workbook.Worksheets.Add(sheetName1);

                // Headings for the first sheet
                worksheet1.Cells[1, 1].Value = "Product Id";
                worksheet1.Cells[1, 2].Value = "Product Name";
                worksheet1.Cells[1, 3].Value = "Price Per Unit";
                worksheet1.Cells[1, 4].Value = "Quantity";
                worksheet1.Cells[1, 5].Value = "Total Price";

                worksheet1.Column(1).Width = 15; // Width for Product Id
                worksheet1.Column(2).Width = 35; // Width for Product Name
                worksheet1.Column(3).Width = 20; // Width for Price Per Unit
                worksheet1.Column(4).Width = 15; // Width for Quantity
                worksheet1.Column(5).Width = 15; // Width for Total Price

                // Data for the first sheet
                int row1 = 2;
                foreach (var kvp in productDictionary)
                {
                    worksheet1.Cells[row1, 1].Value = kvp.Value.product.Id;
                    worksheet1.Cells[row1, 2].Value = kvp.Value.product.Name;
                    worksheet1.Cells[row1, 3].Value = kvp.Value.product.Price;
                    worksheet1.Cells[row1, 4].Value = kvp.Value.quantity;
                    worksheet1.Cells[row1, 5].Value = kvp.Value.totalPrice;
                    row1++;
                }

                // Total sum of all orders
                worksheet1.Cells[row1, 4].Value = "Overall Total Price:";
                worksheet1.Cells[row1, 5].Value = overallTotalPrice;

                // Second sheet
                var worksheet2 = package.Workbook.Worksheets.Add("Employee Product Details");

                // Headings for the second sheet
                worksheet2.Cells[1, 1].Value = "Employee Id";
                worksheet2.Cells[1, 2].Value = "Employee Name";
                worksheet2.Cells[1, 3].Value = "Product Id";
                worksheet2.Cells[1, 4].Value = "Product Name";
                worksheet2.Cells[1, 5].Value = "Order Id";
                worksheet2.Cells[1, 6].Value = "Order Title";
                worksheet2.Cells[1, 7].Value = "Price";
                worksheet2.Cells[1, 8].Value = "Quantity";
                worksheet2.Cells[1, 9].Value = "Total Product Price";
                worksheet2.Cells[1, 10].Value = "Employee Salary";

                worksheet2.Column(1).Width = 10; // Width for Employee Id
                worksheet2.Column(2).Width = 20; // Width for Employee Name
                worksheet2.Column(3).Width = 10; // Width for Product Id
                worksheet2.Column(4).Width = 35; // Width for Product Name
                worksheet2.Column(5).Width = 10; // Width for Order Id
                worksheet2.Column(6).Width = 35; // Width for Order Title
                worksheet2.Column(7).Width = 10; // Width for Price
                worksheet2.Column(8).Width = 10; // Width for Quantity
                worksheet2.Column(9).Width = 20; // Width for Total Product Price
                worksheet2.Column(10).Width = 20; // Width for Employee Salary

                // Data for the second sheet
                int row2 = 2;
                foreach (var employeeGroup in employeeProductList.GroupBy(x => x.employee))
                {
                    var employee = employeeGroup.Key;
                    // Record the employee's name once
                    worksheet2.Cells[row2, 1].Value = employee.Id;
                    worksheet2.Cells[row2, 2].Value = employee.Name;
                    worksheet2.Cells[row2, 10].Value = employee.Salary;
                    row2++;

                    foreach (var item in employeeGroup)
                    {
                        worksheet2.Cells[row2, 3].Value = item.product.Id;
                        worksheet2.Cells[row2, 4].Value = item.product.Name;
                        worksheet2.Cells[row2, 5].Value = item.order.Id;
                        worksheet2.Cells[row2, 6].Value = item.order.Title;
                        worksheet2.Cells[row2, 7].Value = item.price;
                        worksheet2.Cells[row2, 8].Value = item.quantity;
                        worksheet2.Cells[row2, 9].Value = item.totalProductPrice;
                        row2++;
                    }
                }

                return await package.GetAsByteArrayAsync();
            }
        }
    }
}
