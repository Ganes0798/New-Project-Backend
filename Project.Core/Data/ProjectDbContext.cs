using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Core.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Data
{
    public partial class ProjectDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public ProjectDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }

        public DbSet<Login> Registration { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Configuration.GetConnectionString("Postgres"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable(CommonNames.Login);
                entity.Property(e => e.User_id).HasColumnName("user_id");
                entity.Property(e => e.Username).HasColumnName("user_name");
                entity.Property(e => e.Password).HasColumnName("user_pass");
                entity.Property(e => e.Email).HasColumnName("user_emailId");
                entity.Property(e => e.RoleName).HasColumnName("user_role");
                entity.Property(e => e.termAccept).HasColumnName("accept");
                entity.Property(e => e.createdOn).HasColumnName("created_on").HasDefaultValueSql(CommonNames.NOW);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
