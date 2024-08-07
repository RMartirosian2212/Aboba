using System.ComponentModel.DataAnnotations.Schema;

namespace Aboba.Domain.Entities;

public class OrderProduct
{
    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int? ProductId { get; set; }
    public Product Product { get; set; }
    
    public int? EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
    public int Quantity { get; set; }

    [NotMapped] public string ProductName { get; set; }
    [NotMapped] public bool IsInDb { get; set; }

}