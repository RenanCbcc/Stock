using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Back_End.Models.ProductModels
{
    public interface IProductRepository
    {
        Task<Product> Read(int Id);
        Task<Product> Read(string Code);
        IQueryable<Product> Browse();
        Task<IQueryable<Product>> SuggestionsTo(int ClientId);
        Task Add(Product product);
        Task Edit(Product product);
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

        public IQueryable<Product> Browse()
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

        public async Task<Product> Read(string Code)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Code == Code);
        }

        public async Task<IQueryable<Product>> SuggestionsTo(int Id)
        {
            /*
             SELECT alsobought.* from Items itemBought
            JOIN Items alsobought on itemBought.OrderId = alsobought.OrderId
            WHERE itemBought.ProductId = 1 AND itemBought.ProductId != alsobought.ProductId
             */

            var lastOrder = await context.Orders.Where(o => o.Client.Id == Id)
                .OrderByDescending(o => o.Date).FirstAsync();

            var lastPurchases = context.Items.Where(i => i.OrderId == lastOrder.Id)
                .Select(i => i.Id);

            var query = from itemBought in context.Items
                        join alsobought in context.Items
                        on itemBought.OrderId equals alsobought.OrderId
                        where lastPurchases.Contains(itemBought.ProductId)
                        && itemBought.ProductId != alsobought.ProductId
                        select alsobought.Product;

            return query;
        }


    }
}
