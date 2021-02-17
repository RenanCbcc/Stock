using Microsoft.EntityFrameworkCore;
using Stock_Back_End.Models.ProductModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Back_End.Models.CategoryModels
{
    public interface ICategoryRepository
    {
        Task<Category> Read(int Id);
        Task<IEnumerable<Category>> All();
        IQueryable<Category> Browse();
        IQueryable<Product> Browse(int id);
        Task Add(Category category);
        Task Edit(Category category);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly StockContext context;

        public CategoryRepository(StockContext context)
        {
            this.context = context;
        }
        public async Task Add(Category category)
        {
            await context.AddAsync(category);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> All()
        {
            return await context.Categories.ToListAsync();
        }

        public IQueryable<Category> Browse()
        {
            return context.Categories;
        }

        public IQueryable<Product> Browse(int Id)
        {
            //return context.Categories.Include(c => c.Products).Where(c=> c.Id == Id);
            return context.Products.Where(p => p.Category.Id == Id);
        }

        public async Task Edit(Category alteredCategory)
        {
            var category = context.Categories.Attach(alteredCategory);
            category.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<Category> Read(int Id)
        {
            return await context.Categories.FindAsync(Id);
        }
    }
}
