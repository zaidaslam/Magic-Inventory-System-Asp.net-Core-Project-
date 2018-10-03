using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Assignment2WebApi.Models;

namespace Assignment2WebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<OwnerInventory> OwnerInventory { get; set; }
        public DbSet<StockRequest> StockRequests { get; set; }

        public DbSet<OrderHistory> OrdersHistory { get; set; }
       

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<StoreInventory>().HasKey(x => new { x.StoreID, x.ProductID });
            builder.Entity<Cart>().HasKey(x => new { x.CustomerID, x.StoreID, x.ProductID });
            //builder.Entity<OrderHistory>().HasKey(x => new { x.CustomerID, x.OrderID});
            builder.Entity<Order>().HasKey(x => new { x.OrderID, x.ProductID, x.StoreID });

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<StoreInventory> StoreInventory { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Orders { get; set; }
        
    }
}
