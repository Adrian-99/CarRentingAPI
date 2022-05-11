using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentingAPI.Data.Entities
{
    [Table("Rentals")]
    public class Rental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public decimal TotalCost { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid CarId { get; set; }


        public Client Client { get; set; }
        public Car Car { get; set; }
    }
}
