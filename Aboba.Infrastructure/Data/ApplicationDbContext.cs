using Aboba.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aboba.Infrastucture.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Price).HasColumnType("REAL");
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity
                .Property(e => e.TotalPrice).HasColumnType("REAL");
            entity.HasIndex(e => e.Title).IsUnique();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Salary).HasColumnType("REAL");
        });

        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId });

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId);
        
        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Employee)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.EmployeeId)
            .IsRequired(false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=../Aboba.Infrastructure/AbobaDB.db;foreign keys=true;");
    }
}