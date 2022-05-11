namespace CarRentingAPI.Dtos
{
    public class RentalInputDto
    {
        public DateTime From { get; }
        public DateTime To { get; }
        public decimal? TotalCost { get; }
        public Guid ClientId { get; }
        public Guid CarId { get; }

        public RentalInputDto(DateTime from, DateTime to, decimal? totalCost, Guid clientId, Guid carId)
        {
            From = from;
            To = to;
            TotalCost = totalCost;
            ClientId = clientId;
            CarId = carId;
        }
    }
}
