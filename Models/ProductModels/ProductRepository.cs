using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.ProductModels
{
    public interface IProductRepository
    {
        Task<Product> Read(int Id);
        IQueryable<Product> Browse();
        Task Add(Product product);
        Task Edit(Product product);
        IQueryable<Product> Search(SearchProductViewModel model);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly StockContext context;

        public ProductRepository(StockContext context)
        {
            this.context = context;
        }

        public Task Add(Product product)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> Browse()
        {
            throw new NotImplementedException();
        }

        public Task Edit(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Read(int Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> Search(SearchProductViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
