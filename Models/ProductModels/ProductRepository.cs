﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.ProductModels
{
    public interface IProductRepository
    {
        Task<Product> Read(int Id);
        Task<Product> Read(string Code);
        IQueryable<Product> Browse();
        IQueryable<Product> RunningLow();
        IEnumerable<Product> Browse(int id);
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

        public IEnumerable<Product> Browse(int id)
        {
            return context.Products.Where(p => p.CategoryId == id).ToList();
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

        public IQueryable<Product> RunningLow()
        {
            return context.Products.Where(p => p.Quantity < p.MinimumQuantity);
        }
    }
}
