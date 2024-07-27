using System.ComponentModel.DataAnnotations;

namespace Aboba.Domain.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastChange { get; set; }
    
    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    
}