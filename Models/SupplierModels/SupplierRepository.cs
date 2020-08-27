using Estoque.Models.SupplierModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estoque.Models.SupplierModels
{
    public interface ISupplierRepository
    {
        Task<Supplier> Read(int Id);
        IEnumerable<Supplier> Browse();
        Task Add(Supplier supplier);
        Task Edit(Supplier supplier);
    }
    public class SupplierRepository : ISupplierRepository
    {
        private readonly StockContext context;

        public SupplierRepository(StockContext context)
        {
            this.context = context;
        }
        public async Task Add(Supplier supplier)
        {
            await context.AddAsync(supplier);
            await context.SaveChangesAsync();
        }

        public IEnumerable<Supplier> Browse()
        {
            return context.Suppliers;
        }

        public async Task Edit(Supplier alteredSupplier)
        {
            var supplier = context.Suppliers.Attach(alteredSupplier);
            supplier.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<Supplier> Read(int Id)
        {
            return await context.Suppliers.FindAsync(Id);
        }
    }
}
