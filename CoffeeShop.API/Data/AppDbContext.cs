using Microsoft.EntityFrameworkCore;
using CoffeeShop.API.Models;

namespace CoffeeShop.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions opt) : base(opt)
    {

    }

    public DbSet<Menu> Menus { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>()
        .HasMany(m => m.Products)
        .WithOne(s => s.Menu!)
        .HasForeignKey(s => s.MenuId);
    }
}