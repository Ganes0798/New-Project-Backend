﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Core.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.Core.Enums.CommonEnums;

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

        public DbSet<Register> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> categories { get; set; }
        public DbSet<Order> Orders { get; set; }

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

            modelBuilder.Entity<Register>(entity =>
            {
                entity.ToTable(CommonNames.Register);
                entity.Property(e => e.Id).HasColumnName(CommonNames.Id);
                entity.Property(e => e.FirstName).HasColumnName("first_name");
				entity.Property(e => e.LastName).HasColumnName("last_name");
				entity.Property(e => e.Password).HasColumnName("user_pass");
                entity.Property(e => e.Email).HasColumnName("email_id");
                entity.Property(e => e.RoleName).HasColumnName("role");
                entity.Property(e => e.CreatedOn).HasColumnName(CommonNames.CreatedOn).HasDefaultValueSql(CommonNames.NOW);
                entity.Property(e => e.ModifiedOn).HasColumnName(CommonNames.ModifiedOn).HasDefaultValueSql(CommonNames.NOW);
				entity.Property(e => e.DataState).HasColumnName(CommonNames.DataState).HasDefaultValue(RecordState.Active);
			});

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable(CommonNames.Product);
                entity.Property(e => e.Id).HasColumnName(CommonNames.Id);
                entity.Property(e => e.ProductName).HasColumnName("name");
                entity.Property(e => e.UserFkId).HasColumnName(CommonNames.UserFkId);
                entity.Property(e => e.ProductDescription).HasColumnName("description").HasMaxLength(8129);
                entity.Property(e => e.ProductImageUrl).HasColumnName("ProductImage Url");
                entity.Property(e => e.ProductQuantity).HasColumnName("quantity");
                entity.Property(e => e.CategoryCode).HasColumnName("category_fk_id");
				entity.Property(e => e.CreatedOn).HasColumnName(CommonNames.CreatedOn).HasDefaultValueSql(CommonNames.NOW);
				entity.Property(e => e.ModifiedOn).HasColumnName(CommonNames.ModifiedOn).HasDefaultValueSql(CommonNames.NOW);
				entity.Property(e => e.DataState).HasColumnName(CommonNames.DataState).HasDefaultValue(RecordState.Active);
			});

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable(CommonNames.Category);
                entity.Property(e => e.Id).HasColumnName(CommonNames.Id);
                entity.Property(e => e.Name).HasColumnName("category_name");
                entity.Property(e => e.Code).HasColumnName("category_code");
                entity.Property(e => e.CreatedOn).HasColumnName(CommonNames.CreatedOn).HasDefaultValueSql(CommonNames.NOW);
                entity.Property(e => e.ModifiedOn).HasColumnName(CommonNames.ModifiedOn).HasDefaultValueSql(CommonNames.NOW);
				entity.Property(e => e.DataState).HasColumnName(CommonNames.DataState).HasDefaultValue(RecordState.Active);
			});

            modelBuilder.Entity<Order>(entity =>
            {
				entity.ToTable(CommonNames.Order);
				entity.Property(e => e.Id).HasColumnName(CommonNames.Id);
				entity.Property(e => e.UserFkId).HasColumnName(CommonNames.UserFkId);
				entity.Property(e => e.ProductFkId).HasColumnName(CommonNames.ProductFkId);
				entity.Property(e => e.CreatedOn).HasColumnName(CommonNames.CreatedOn).HasDefaultValueSql(CommonNames.NOW);
				entity.Property(e => e.ModifiedOn).HasColumnName(CommonNames.ModifiedOn).HasDefaultValueSql(CommonNames.NOW);
				entity.Property(e => e.DataState).HasColumnName(CommonNames.DataState).HasDefaultValue(RecordState.Active);
			});


            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
