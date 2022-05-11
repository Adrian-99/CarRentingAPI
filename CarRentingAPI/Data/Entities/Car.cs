using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentingAPI.Data.Entities
{
    [Table("Cars")]
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public decimal RentCostPerDay { get; set; }


        public ICollection<Rental> Rentals { get; set; }
    }
}
