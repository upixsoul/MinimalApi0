using Microsoft.EntityFrameworkCore;
using Minimal.REPR.Models;

namespace Minimal.REPR
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
    }
}
