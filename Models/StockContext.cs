using Estoque.Models.ProductModels;
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
        
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {

        }
    }
}
