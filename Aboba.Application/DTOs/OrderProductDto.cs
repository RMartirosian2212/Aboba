namespace Aboba.Application.DTOs;

public class OrderProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int OrderId { get; set; }
    public string OrderTitle { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}