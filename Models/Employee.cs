using System;
using System.ComponentModel.DataAnnotations;

namespace WebProject.Models
{
	public enum AuthLevel
	{
		Admin,
		User
	}

	public class Employee
	{
		[Key]
		public int EmployeeId { get; set; }

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
		[MaxLength(100)]
		public string Position { get; set; }

		[DataType(DataType.Currency)]
		public decimal Salary { get; set; }

		[Required]
		[MaxLength(15)]
		public string Phone { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		public DateTime HireDate { get; set; } = DateTime.Now;

		[Required]
		public AuthLevel AuthLevel { get; set; }

		[MaxLength(200)]
		public string Address { get; set; }

		[MaxLength(500)]
		public string ProfileImageUrl { get; set; }

		public bool IsActive { get; set; } = true;

		[MaxLength(1000)]
		public string WorkHours { get; set; }

		// Navigation Properties
		public int SalonId { get; set; } // Foreign Key
		public Salon Salon { get; set; } // salon of employee

		public ICollection<Appointment> Appointments { get; set; } // appointments of employee
	}

}
