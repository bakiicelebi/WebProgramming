using Microsoft.EntityFrameworkCore;

namespace WebProject.Models
{
	public class DataBaseContext : DbContext
	{
		public DataBaseContext(DbContextOptions<DataBaseContext> options)
			: base(options)
		{ }

		// DbSets (Entities)
		public DbSet<DbTest> DbTests { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Service> Services { get; set; }
		public DbSet<Salon> Salons { get; set; }
		public DbSet<Appointment> Appointments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Decimal fields precision and scale configuration
			modelBuilder.Entity<Employee>()
				.Property(e => e.Salary)
				.HasColumnType("decimal(18,2)");  // Precision and scale for Salary

			modelBuilder.Entity<Service>()
				.Property(s => s.Price)
				.HasColumnType("decimal(18,2)");  // Precision and scale for Price

			modelBuilder.Entity<Appointment>()
				.Property(a => a.TotalPrice)
				.HasColumnType("decimal(18,2)");  // Precision and scale for TotalPrice

			// Employee - Salon relationship (One Employee belongs to one Salon, a Salon has many Employees)
			modelBuilder.Entity<Employee>()
				.HasOne(e => e.Salon) // One Employee to one Salon
				.WithMany(s => s.Employees) // One Salon has many Employees
				.HasForeignKey(e => e.SalonId) // Foreign Key: Employee has SalonId
				.OnDelete(DeleteBehavior.Restrict);  // Prevent deleting a Salon if Employees exist

			// Appointment - Employee relationship (One Appointment is assigned to one Employee, an Employee has many Appointments)
			modelBuilder.Entity<Appointment>()
				.HasOne(a => a.Employee) // One Appointment to one Employee
				.WithMany(e => e.Appointments) // One Employee has many Appointments
				.HasForeignKey(a => a.EmployeeId) // Foreign Key: Appointment has EmployeeId
				.OnDelete(DeleteBehavior.Cascade);  // Cascade delete: if Appointment is deleted, related Employee stays

			// Appointment - Customer relationship (One Appointment is assigned to one Customer, a Customer has many Appointments)
			modelBuilder.Entity<Appointment>()
				.HasOne(a => a.Customer) // One Appointment to one Customer
				.WithMany(c => c.Appointments) // One Customer has many Appointments
				.HasForeignKey(a => a.CustomerId) // Foreign Key: Appointment has CustomerId
				.OnDelete(DeleteBehavior.Cascade);  // Cascade delete: if Appointment is deleted, related Customer stays

			// Appointment - Service relationship (One Appointment is for one Service, a Service can have many Appointments)
			modelBuilder.Entity<Appointment>()
				.HasOne(a => a.Service) // One Appointment to one Service
				.WithMany(s => s.Appointments) // One Service has many Appointments
				.HasForeignKey(a => a.ServiceId) // Foreign Key: Appointment has ServiceId
				.OnDelete(DeleteBehavior.Restrict);  // Prevent deleting a Service if it has related Appointments

			// Service - Salon relationship (One Service is offered by one Salon, a Salon offers many Services)
			modelBuilder.Entity<Service>()
				.HasOne(s => s.Salon) // One Service belongs to one Salon
				.WithMany(sa => sa.Services) // One Salon offers many Services
				.HasForeignKey(s => s.SalonId) // Foreign Key: Service has SalonId
				.OnDelete(DeleteBehavior.Restrict);  // Prevent deleting a Salon if it has related Services
		}

	}
}
