using Microsoft.EntityFrameworkCore;

namespace ApiTest.Entity
{
    public class AppDbContext : DbContext
    {
        // public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductEntity> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
