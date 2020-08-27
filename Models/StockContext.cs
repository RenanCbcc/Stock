using Estoque.Models.CategoryModels;
using Estoque.Models.ClientModels;
using Estoque.Models.ProductModels;
using Estoque.Models.SupplierModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models
{
    public class StockContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {

        }
    }
}
