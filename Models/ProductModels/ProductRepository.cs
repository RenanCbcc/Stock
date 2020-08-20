using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.ProductModels
{
    public interface IProductRepository
    {
        Task<Product> Read(int Id);
        IEnumerable<Product> Browse();
        Task Add(Product product);
        Task Edit(Product product);
        IQueryable<Product> Search(SearchViewModel model);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly StockContext context;

        public ProductRepository(StockContext context)
        {
            this.context = context;
        }

        public async Task Add(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public IEnumerable<Product> Browse()
        {
            return context.Products;
        }

        public async Task Edit(Product alteredProduct)
        {
            var product = context.Products.Attach(alteredProduct);
            product.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<Product> Read(int Id)
        {
            return await context.Products.FindAsync(Id);
        }

        public IQueryable<Product> Search(SearchViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
