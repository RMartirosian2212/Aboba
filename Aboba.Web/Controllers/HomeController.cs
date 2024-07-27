using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Aboba.Services;
using OfficeOpenXml;

namespace Aboba.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
}