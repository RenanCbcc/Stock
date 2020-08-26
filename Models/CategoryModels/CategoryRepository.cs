using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.CategoryModels
{
    public interface ICategoryRepository
    {
        Task<Category> Read(int Id);
        IEnumerable<Category> Browse();
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

        public IEnumerable<Category> Browse()
        {
            return context.Categories;
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
