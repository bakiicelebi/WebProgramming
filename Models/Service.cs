using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebProject.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }


        [AllowNull]
        public string? ImageUrl { get; set; }


        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public TimeSpan Duration { get; set; } // service duration

        public int SalonId { get; set; } // Foreign Key
        public Salon Salon { get; set; } // salon of service

        public ICollection<Appointment> Appointments { get; set; } // appointments associated with service
    }

}
