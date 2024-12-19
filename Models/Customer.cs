using System.ComponentModel.DataAnnotations;
namespace WebProject.Models
{

	public class Customer
	{
		[Key]
		public int CustomerId { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Required]
		[MaxLength(50)]
		public string Username { get; set; }

		[Required]
		[MaxLength(255)]
		public string Password { get; set; }

		[Required]
		[MaxLength(15)]
		public string Phone { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[MaxLength(500)]
		public string ProfileImageUrl { get; set; } // can be used for profile photo

		public ICollection<Appointment> Appointments { get; set; } // appointments of customer
	}

}
