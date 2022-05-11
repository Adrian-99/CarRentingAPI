namespace CarRentingAPI.Dtos
{
    public class ClientDto
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string PhoneNumber { get; }

        public ClientDto(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }
    }
}
