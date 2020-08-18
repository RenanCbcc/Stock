using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.ClientModels
{
    public interface IClientRepository
    {
        Task<Client> Read(int Id);
        IQueryable<Client> Browse();
        Task Add(Client client);
        Task Edit(Client client);
        IQueryable<Client> Search(SearchClientViewModel model);

    }

    public class ClientRepository : IClientRepository
    {
        private readonly StockContext context;

        public ClientRepository(StockContext context)
        {
            this.context = context;
        }

        public Task Add(Client client)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Client> Browse()
        {
            throw new NotImplementedException();
        }

        public Task Edit(Client client)
        {
            throw new NotImplementedException();
        }

        public Task<Client> Read(int Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Client> Search(SearchClientViewModel modelo)
        {
            throw new NotImplementedException();
        }
    }
}
