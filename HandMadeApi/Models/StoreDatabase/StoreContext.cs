using Microsoft.EntityFrameworkCore;

namespace HandMadeApi.Models.StoreDatabase
{
    //Install the following Packages
    //Install-Package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
    //Install-Package Microsoft.EntityFrameworkCore.SqlServer
    public partial class StoreContext : DbContext
    {
        //Connection String => 
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductRate> ProductRates { get; set; }


        //To prevent Polarized table names
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>().ToTable("Store");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Order>().ToTable("ProductRate");
        }

    }
}
