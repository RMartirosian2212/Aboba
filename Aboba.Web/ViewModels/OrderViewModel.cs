using Aboba.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aboba.ViewModels;

public class OrderViewModel
{
    public List<OrderProduct> OrderProducts { get; set; }
    public List<SelectListItem> Employees { get; set; }
    public Dictionary<int, int?> SelectedEmployees { get; set; } = new Dictionary<int, int?>();
}
