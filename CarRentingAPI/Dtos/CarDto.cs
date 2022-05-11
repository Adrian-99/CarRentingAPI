namespace CarRentingAPI.Dtos
{
    public class CarDto
    {
        public string Brand { get; }
        public string Model { get; }
        public bool IsAvailable { get; }
        public decimal RentCostPerDay { get; }

        public CarDto(string brand, string model, bool isAvailable, decimal rentCostPerDay)
        {
            Brand = brand;
            Model = model;
            IsAvailable = isAvailable;
            RentCostPerDay = rentCostPerDay;
        }
    }
}
