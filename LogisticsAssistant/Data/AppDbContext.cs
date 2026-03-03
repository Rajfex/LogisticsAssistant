using Microsoft.EntityFrameworkCore;
using LogisticsAssistant.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Truck> Trucks { get; set; }
    public DbSet<TruckRoute> Routes { get; set; }
}