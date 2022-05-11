using CarRentingAPI.Data;
using CarRentingAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentingAPI.Repositories
{
    public class RentalRepository
    {
        private CarRentingContext carRentingContext;

        public RentalRepository(CarRentingContext carRentingContext)
        {
            this.carRentingContext = carRentingContext;
        }

        public async Task<Rental?> GetById(Guid id)
        {
            return await (from rental in carRentingContext.Rentals
                          .Include(r => r.Car)
                          .Include(r => r.Client)
                          where rental.Id == id
                          select rental).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Rental>> GetAll()
        {
            return await carRentingContext.Rentals
                .Include(r => r.Car)
                .Include(r => r.Client)
                .ToListAsync();
        }

        public async Task<Rental> Create(Rental rental)
        {
            var addedRental = await carRentingContext.Rentals.AddAsync(rental);
            await carRentingContext.SaveChangesAsync();
            return addedRental.Entity;
        }

        public async Task<Rental> Update(Rental rental)
        {
            var updatedRental = carRentingContext.Rentals.Update(rental);
            await carRentingContext.SaveChangesAsync();
            return updatedRental.Entity;
        }

        public async Task Delete(Guid id)
        {
            var rental = await GetById(id);
            if (rental != null)
            {
                carRentingContext.Rentals.Remove(rental);
                await carRentingContext.SaveChangesAsync();
            }
        }
    }
}
