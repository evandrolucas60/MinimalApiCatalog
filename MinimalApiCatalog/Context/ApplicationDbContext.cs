using Microsoft.EntityFrameworkCore;
using MinimalApiCatalog.Models;

namespace MinimalApiCatalog.Context
{
    public class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            //Category
            mb.Entity<Category>().HasKey(c => c.CategoryId);

            mb.Entity<Category>().Property(c => c.Name)
                                 .HasMaxLength(100)
                                 .IsRequired();

            mb.Entity<Category>().Property(c => c.Description).HasMaxLength(150).IsRequired();

            //product
            mb.Entity<Product>().HasKey(p => p.ProductId);
            mb.Entity<Product>().Property(p => p.Name).HasMaxLength(100).IsRequired();
            mb.Entity<Product>().Property(p => p.Description).HasMaxLength(150);
            mb.Entity<Product>().Property(p => p.Image).HasMaxLength(100);

            mb.Entity<Product>().Property(p => p.Price).HasPrecision(14,2);

            //Relacionship
            mb.Entity<Product>()
                .HasOne<Category>(c => c.Category)
                .WithMany(c => c.Products).HasForeignKey(c=> c.CategoryId);
        }
    }
}
