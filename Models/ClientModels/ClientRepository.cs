using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Back_End.Models.ClientModels
{
    public interface IClientRepository
    {
        Task<Client> Read(int Id);
        IQueryable<Client> Browse();
        IQueryable<Client> inactive();
        Task Add(Client client);
        Task Edit(Client client);
    }

    public class ClientRepository : IClientRepository
    {
        private readonly StockContext context;

        public ClientRepository(StockContext context)
        {
            this.context = context;
        }

        public async Task Add(Client client)
        {
            await context.AddAsync(client);
            await context.SaveChangesAsync();
        }

        public IQueryable<Client> Browse()
        {
            return context.Clients;
        }

        public async Task Edit(Client alteredClient)
        {
            var client = context.Clients.Attach(alteredClient);
            client.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public IQueryable<Client> inactive()
        {
            return context.Clients.Where(c => c.Status == Status.Inativo);
        }

        public async Task<Client> Read(int Id)
        {
            return await context.Clients.FindAsync(Id);
        }

    }
}
