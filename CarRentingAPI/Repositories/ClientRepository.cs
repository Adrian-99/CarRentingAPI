using CarRentingAPI.Data;
using CarRentingAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentingAPI.Repositories
{
    public class ClientRepository
    {
        private CarRentingContext carRentingContext;

        public ClientRepository(CarRentingContext carRentingContext)
        {
            this.carRentingContext = carRentingContext;
        }

        public async Task<Client?> GetById(Guid id)
        {
            return await (from client in carRentingContext.Clients
                          where client.Id == id
                          select client).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Client>> GetAll()
        {
            return await carRentingContext.Clients.ToListAsync();
        }

        public async Task<Client> Create(Client client)
        {
            var addedClient = await carRentingContext.Clients.AddAsync(client);
            await carRentingContext.SaveChangesAsync();
            return addedClient.Entity;
        }

        public async Task<Client> Update(Client client)
        {
            var updatedClient = carRentingContext.Clients.Update(client);
            await carRentingContext.SaveChangesAsync();
            return updatedClient.Entity;
        }

        public async Task Delete(Guid id)
        {
            var client = await GetById(id);
            if (client != null)
            {
                carRentingContext.Clients.Remove(client);
                await carRentingContext.SaveChangesAsync();
            }
        }
    }
}
