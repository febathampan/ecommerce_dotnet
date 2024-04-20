using Microsoft.EntityFrameworkCore;
using test1app.Models;

namespace test1app.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // Additional configuration or initialization if needed
    }

    public DbSet<test1app.Models.Product> Product { get; set; } = default!;

    public DbSet<test1app.Models.Cart> Cart { get; set; } = default!;

    public DbSet<test1app.Models.User> User { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure one-to-one relationship between Cart and User
        modelBuilder
            .Entity<Cart>()
            .HasOne(c => c.User) // Cart has one User
            .WithMany() // User can have multiple Carts
            .HasForeignKey(c => c.UserId); // Foreign key property in Cart

        base.OnModelCreating(modelBuilder);
    }
}
