using System.ComponentModel.DataAnnotations;

namespace Aboba.Domain.Entities;

public class EmployeeProduct
{
    
    [Required]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }
}