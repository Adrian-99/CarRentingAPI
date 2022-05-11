using CarRentingAPI.Data.Entities;
using CarRentingAPI.Dtos;
using CarRentingAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingAPI.Controllers
{
    [Route("api/rental")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private CarRepository carRepository;
        private ClientRepository clientRepository;
        private RentalRepository rentalRepository;

        public RentalController(CarRepository carRepository, ClientRepository clientRepository, RentalRepository rentalRepository)
        {
            this.carRepository = carRepository;
            this.clientRepository = clientRepository;
            this.rentalRepository = rentalRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<RentalOutputDto>>> Get()
        {
            var rentals = (await rentalRepository.GetAll())
                .Select(rental => new RentalOutputDto(
                    rental.From,
                    rental.To,
                    rental.TotalCost,
                    new ClientDto(rental.Client.FirstName, rental.Client.LastName, rental.Client.PhoneNumber),
                    new CarDto(rental.Car.Brand, rental.Car.Model, rental.Car.IsAvailable, rental.Car.RentCostPerDay)
                    ));
            return Ok(rentals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalOutputDto>> Get(Guid id)
        {
            var rental = await rentalRepository.GetById(id);
            if (rental == null)
            {
                return BadRequest($"Rental with id {id} doesn't exist");
            }
            return Ok(new RentalOutputDto(
                    rental.From,
                    rental.To,
                    rental.TotalCost,
                    new ClientDto(rental.Client.FirstName, rental.Client.LastName, rental.Client.PhoneNumber),
                    new CarDto(rental.Car.Brand, rental.Car.Model, rental.Car.IsAvailable, rental.Car.RentCostPerDay)
                    ));
        }

        [HttpPost]
        public async Task<ActionResult<RentalOutputDto>> Post([FromBody] RentalInputDto rentalInputDto)
        {
            var car = await carRepository.GetById(rentalInputDto.CarId);
            var client = await clientRepository.GetById(rentalInputDto.ClientId);
            if (car == null)
            {
                return BadRequest($"Car with id {rentalInputDto.CarId} doesn't exist");
            }
            if (client == null)
            {
                return BadRequest($"Client with id {rentalInputDto.ClientId} doesn't exist");
            }
            if (!car.IsAvailable)
            {
                return BadRequest($"Car with id {rentalInputDto.CarId} is currently not available for renting");
            }
            var carAvailabilityError = CheckCarAvailabilityAtGivenDates(rentalInputDto, car);
            if (carAvailabilityError != null)
            {
                return BadRequest(carAvailabilityError);
            }
            var rental = new Rental()
            {
                From = rentalInputDto.From,
                To = rentalInputDto.To,
                TotalCost = EvaluateTotalCost(rentalInputDto, car),
                Client = client,
                Car = car
            };
            var createdRental = await rentalRepository.Create(rental);
            return Created(
                $"/api/rental/{createdRental.Id}",
                new RentalOutputDto(
                    createdRental.From,
                    createdRental.To,
                    createdRental.TotalCost,
                    new ClientDto(createdRental.Client.FirstName, createdRental.Client.LastName, createdRental.Client.PhoneNumber),
                    new CarDto(createdRental.Car.Brand, createdRental.Car.Model, createdRental.Car.IsAvailable, createdRental.Car.RentCostPerDay)
                    )
                );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RentalOutputDto>> Put(Guid id, [FromBody] RentalInputDto rentalInputDto)
        {
            var rental = await rentalRepository.GetById(id);
            if (rental != null)
            {
                var car = await carRepository.GetById(rentalInputDto.CarId);
                var client = await clientRepository.GetById(rentalInputDto.ClientId);
                if (car == null)
                {
                    return BadRequest($"Car with id {rentalInputDto.CarId} doesn't exist");
                }
                if (client == null)
                {
                    return BadRequest($"Client with id {rentalInputDto.ClientId} doesn't exist");
                }
                if (!car.IsAvailable)
                {
                    return BadRequest($"Car with id {rentalInputDto.CarId} is currently not available");
                }
                var carAvailabilityError = CheckCarAvailabilityAtGivenDates(rentalInputDto, car);
                if (carAvailabilityError != null)
                {
                    return BadRequest(carAvailabilityError);
                }
                rental.From = rentalInputDto.From;
                rental.To = rentalInputDto.To;
                rental.TotalCost = EvaluateTotalCost(rentalInputDto, car);
                rental.Client = client;
                rental.Car = car;
                var updatedRental = await rentalRepository.Update(rental);
                return Ok(new RentalOutputDto(
                    updatedRental.From,
                    updatedRental.To,
                    updatedRental.TotalCost,
                    new ClientDto(updatedRental.Client.FirstName, updatedRental.Client.LastName, updatedRental.Client.PhoneNumber),
                    new CarDto(updatedRental.Car.Brand, updatedRental.Car.Model, updatedRental.Car.IsAvailable, updatedRental.Car.RentCostPerDay)
                    ));
            }
            else
            {
                return BadRequest($"Rental with id {id} doesn't exist");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var rental = await rentalRepository.GetById(id);
            if (rental != null)
            {
                await rentalRepository.Delete(id);
                return NoContent();
            }
            else
            {
                return BadRequest($"Rental with id {id} doesn't exist");
            }
        }

        private decimal EvaluateTotalCost(RentalInputDto rentalInputDto, Car car)
        {
            if (rentalInputDto.TotalCost != null)
            {
                return (decimal)rentalInputDto.TotalCost;
            }
            return (decimal)rentalInputDto.To.Subtract(rentalInputDto.From).TotalDays * car.RentCostPerDay;
        }

        private string? CheckCarAvailabilityAtGivenDates(RentalInputDto rentalInputDto, Car car)
        {
            foreach (var rental in car.Rentals)
            {
                var maxStart = rental.From.CompareTo(rentalInputDto.From) > 0 ? rental.From : rentalInputDto.From;
                var minTo = rental.To.CompareTo(rentalInputDto.To) < 0 ? rental.To : rentalInputDto.To;

                if (maxStart < minTo)
                {
                    return $"Car with id {rentalInputDto.CarId} will not be available from {rental.From} to {rental.To}";
                }
            }
            return null;
        }
    }
}
