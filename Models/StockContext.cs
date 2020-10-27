using Estoque.Models.CategoryModels;
using Estoque.Models.ClientModels;
using Estoque.Models.OrderModels;
using Estoque.Models.PaymentModels;
using Estoque.Models.ProductModels;
using Estoque.Models.SupplierModels;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Models
{
    public class StockContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
