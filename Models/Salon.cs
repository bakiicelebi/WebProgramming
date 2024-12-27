using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebProject.Models
{
	public class Salon
	{
		[Key]
		public int SalonId { get; set; }

		[Required]
		[MaxLength(150)]
		public string Name { get; set; }


		[AllowNull]
		[MaxLength(20)]
		public string Phone { get; set; }

		[MaxLength(500)]
		public string LogoUrl { get; set; }

		[AllowNull]
		public string OpeningHours { get; set; } // can be json

		public ICollection<Employee> Employees { get; set; } // employees of salon
		public ICollection<Service> Services { get; set; } // services of salon
	}
}
