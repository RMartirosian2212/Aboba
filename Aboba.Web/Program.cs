using Aboba.Application.Handlers.Product;
using Aboba.Application.Interfaces;
using Aboba.Application.Services;
using Aboba.Infrastucture.Data;
using Aboba.Infrastucture.Data.Repository;
using Aboba.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>();

var services = builder.Services;
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProductsQueryHandler).Assembly))
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IOrderRepository, OrderRepository>()
    .AddScoped<IOrderProductRepository, OrderProductRepository>()
    .AddScoped<IEmployeeRepository, EmployeeRepository>()
    .AddScoped<IOrderService, OrderService>()
    .AddScoped<IExcelOrderProductProcessor, ExcelOrderProductProcessor>()
    .AddScoped<IOrderExportService, OrderExportService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();