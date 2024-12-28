using System.ComponentModel.DataAnnotations;

namespace WebProject.Models
{


	public class Appointment
	{
		[Key]
		public int AppointmentId { get; set; }

		[Required]
		public int EmployeeId { get; set; } // Foreign Key
		public Employee Employee { get; set; } // Employee of appointment

		public int? CustomerId { get; set; } // Foreign Key
		public Customer Customer { get; set; } // customer of appointment

		[Required]
		public int ServiceId { get; set; } // Foreign Key
		public Service Service { get; set; } // service

		[Required]
		public DateTime AppointmentTime { get; set; } // appoint. date

		[MaxLength(500)]
		public string? Notes { get; set; }

		public decimal TotalPrice { get; set; } // Total Price

		public bool? IsConfirmed { get; set; } = null; // Appointment isConfirmed status
	}

}
