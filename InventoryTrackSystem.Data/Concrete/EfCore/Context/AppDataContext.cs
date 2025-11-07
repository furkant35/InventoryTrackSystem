using Microsoft.EntityFrameworkCore;
using InventoryTrackSystem.Model.Concrete;

namespace InventoryTrackSystem.Data.Concrete.EfCore.Context
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Brand>(e =>
            {
                e.ToTable("Brand");
                e.Property(x => x.Name).HasMaxLength(120).IsRequired();
                e.Property(x => x.Desc).HasMaxLength(500);

                e.HasMany(b => b.Products)
                 .WithOne(p => p.Brand)
                 .HasForeignKey(p => p.BrandId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("Product");
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.Code).HasMaxLength(50).IsRequired();
                e.Property(x => x.Description).HasMaxLength(500);
                e.Property(x => x.Price).HasPrecision(18, 2);

                e.HasOne(x => x.Brand)
                 .WithMany(b => b.Products)
                 .HasForeignKey(x => x.BrandId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.BrandId, x.Code }).IsUnique();
            });

            // ---------- USER (varsa) ----------
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
            });
        }
    }
}
