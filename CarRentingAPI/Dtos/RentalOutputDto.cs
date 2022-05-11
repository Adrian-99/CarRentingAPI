namespace CarRentingAPI.Dtos
{
    public class RentalOutputDto
    {
        public DateTime From { get; }
        public DateTime To { get; }
        public decimal TotalCost { get; }
        public ClientDto Client { get; }
        public CarDto Car { get; }

        public RentalOutputDto(DateTime from, DateTime to, decimal totalCost, ClientDto client, CarDto car)
        {
            From = from;
            To = to;
            TotalCost = totalCost;
            Client = client;
            Car = car;
        }
    }
}
