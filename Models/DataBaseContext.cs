using Microsoft.EntityFrameworkCore;

namespace WebProject.Models
{



	public class DataBaseContext : DbContext
	{
		public DbSet<DbTest> dbTests { get; set; }


		public DataBaseContext(DbContextOptions options) : base(options) { }

	}
}
