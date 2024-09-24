using Microsoft.EntityFrameworkCore;
using RetailApp.Models;

namespace RetailApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ApprovalQueue> ApprovalQueue { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
