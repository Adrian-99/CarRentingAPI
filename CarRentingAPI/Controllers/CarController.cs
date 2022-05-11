using CarRentingAPI.Data.Entities;
using CarRentingAPI.Dtos;
using CarRentingAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRentingAPI.Controllers
{
    [Route("api/car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private CarRepository carRepository;

        public CarController(CarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<CarDto>>> Get()
        {
            var cars = (await carRepository.GetAll())
                .Select(car => new CarDto(car.Brand, car.Model, car.IsAvailable, car.RentCostPerDay));
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> Get(Guid id)
        {
            var car = await carRepository.GetById(id);
            if (car == null)
            {
                return BadRequest($"Car with id {id} doesn't exist");
            }
            return Ok(new CarDto(car.Brand, car.Model, car.IsAvailable, car.RentCostPerDay));
        }

        [HttpPost]
        public async Task<ActionResult<CarDto>> Post([FromBody] CarDto carDto)
        {
            var car = new Car()
            {
                Brand = carDto.Brand,
                Model = carDto.Model,
                IsAvailable = carDto.IsAvailable,
                RentCostPerDay = carDto.RentCostPerDay
            };
            var createdCar = await carRepository.Create(car);
            return Created(
                $"/api/car/{createdCar.Id}",
                new CarDto(createdCar.Brand, createdCar.Model, createdCar.IsAvailable, createdCar.RentCostPerDay)
                );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CarDto>> Put(Guid id, [FromBody] CarDto carDto)
        {
            var car = await carRepository.GetById(id);
            if (car != null)
            {
                car.Brand = carDto.Brand;
                car.Model = carDto.Model;
                car.IsAvailable = carDto.IsAvailable;
                car.RentCostPerDay = carDto.RentCostPerDay;
                var updatedCar = await carRepository.Update(car);
                return Ok(new CarDto(updatedCar.Brand, updatedCar.Model, updatedCar.IsAvailable, updatedCar.RentCostPerDay));
            }
            else
            {
                return BadRequest($"Car with id {id} doesn't exist");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var car = await carRepository.GetById(id);
            if (car != null)
            {
                await carRepository.Delete(id);
                return NoContent();
            }
            else
            {
                return BadRequest($"Car with id {id} doesn't exist");
            }
        }
    }
}
