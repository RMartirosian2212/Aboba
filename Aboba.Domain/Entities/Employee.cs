using System.ComponentModel.DataAnnotations;

namespace Aboba.Domain.Entities;

public class Employee
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Salary { get; set; }
    
    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}