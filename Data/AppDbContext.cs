using Microsoft.EntityFrameworkCore;
using Mitra.Models;
using System;
namespace Mitra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet for your custom entities
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Product>().HasKey(p => p.ProductId);
            //modelBuilder.Entity<Product>().HasOne<Category>()
            //    .WithMany()
            //    .HasForeignKey(p => p.CategoryId)
            //    .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Product)
                .WithMany() // Product does not need a navigation property to OrderDetails
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetails>()
                .HasOne(od => od.Order)
                .WithMany() // Assuming no navigation property in Order to OrderDetails
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}