using CarRentingAPI.Data;
using CarRentingAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentingAPI.Repositories
{
    public class CarRepository
    {
        private CarRentingContext carRentingContext;

        public CarRepository(CarRentingContext carRentingContext)
        {
            this.carRentingContext = carRentingContext;
        }

        public async Task<Car?> GetById(Guid id)
        {
            return await (from car in carRentingContext.Cars
                          .Include(c => c.Rentals)
                          where car.Id == id
                          select car).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Car>> GetAll()
        {
            return await carRentingContext.Cars.ToListAsync();
        }

        public async Task<Car> Create(Car car)
        {
            var addedCar = await carRentingContext.Cars.AddAsync(car);
            await carRentingContext.SaveChangesAsync();
            return addedCar.Entity;
        }

        public async Task<Car> Update(Car car)
        {
            var updatedCar = carRentingContext.Cars.Update(car);
            await carRentingContext.SaveChangesAsync();
            return updatedCar.Entity;
        }

        public async Task Delete(Guid id)
        {
            var car = await GetById(id);
            if (car != null)
            {
                carRentingContext.Cars.Remove(car);
                await carRentingContext.SaveChangesAsync();
            }
        }
    }
}
