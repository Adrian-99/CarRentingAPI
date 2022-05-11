using CarRentingAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentingAPI.Data
{
    public class CarRentingContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        public CarRentingContext(DbContextOptions<CarRentingContext> options) :
            base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rentals);
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Rentals);
        }
    }
}
