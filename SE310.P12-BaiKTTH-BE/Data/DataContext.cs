using Microsoft.EntityFrameworkCore;
using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products) // Updated to Products
                .HasForeignKey(p => p.CategoryId);
        }
    }
}