using Microsoft.EntityFrameworkCore;
using New_Project_Backend.Model;

namespace New_Project_Backend.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
		{ 
		}

		public DbSet<Login> Registration { get; set; }
	}
}
