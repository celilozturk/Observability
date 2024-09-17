using Microsoft.EntityFrameworkCore;

namespace Observability1.API.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    

    public DbSet<Product> Products { get; set; } = default;
}
