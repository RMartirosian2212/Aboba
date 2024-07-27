using System.ComponentModel.DataAnnotations;

namespace Aboba.Domain.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    public DateTime UploadDate { get; set; }
    public DateTime LastChange { get; set; }

    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}