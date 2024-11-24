using Microsoft.EntityFrameworkCore;
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

        public DbSet<CartDetails> cart { get; set; }
        public DbSet<Billings> billings { get; set; }
        public DbSet<BillingProduct> billing_products { get; set; }
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
                entity.Property(e => e.ProductDescription).HasColumnName("description").HasMaxLength(8129);
                entity.Property(e => e.ProductImageUrl).HasColumnName("image_url");
                entity.Property(e => e.ProductPrice).HasColumnName("product_price");
                entity.Property(e => e.TotalProducts).HasColumnName("total_products");
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

            modelBuilder.Entity<CartDetails>(entity =>
            {
                entity.ToTable(CommonNames.CartDetails);
                entity.Property(e => e.Id).HasColumnName(CommonNames.Id);
                entity.Property(e => e.ProductFkId).HasColumnName("product_fk_id");
                entity.Property(e => e.UserFkId).HasColumnName("user_fk_id");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.CreatedOn).HasColumnName(CommonNames.CreatedOn).HasDefaultValueSql(CommonNames.NOW);
                entity.Property(e => e.ModifiedOn).HasColumnName(CommonNames.ModifiedOn).HasDefaultValueSql(CommonNames.NOW);
                entity.Property(e => e.DataState).HasColumnName(CommonNames.DataState).HasDefaultValue(RecordState.Active);
            });

            modelBuilder.Entity<Billings>(entity =>
            {

                entity.ToTable("billings");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.BillingReferenceNo).HasColumnName("billing_ref_no");
                entity.Property(e => e.BillingDate).HasColumnName("billing_date");
                entity.Property(e => e.CustomerFkId).HasColumnName("customer_fk");
                entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
                entity.Property(e => e.CreatedOn).HasColumnName(CommonNames.CreatedOn).HasDefaultValueSql(CommonNames.NOW);
                entity.Property(e => e.ModifiedOn).HasColumnName(CommonNames.ModifiedOn).HasDefaultValueSql(CommonNames.NOW);
                entity.Property(e => e.DataState).HasColumnName(CommonNames.DataState).HasDefaultValue(RecordState.Active);
            });

            modelBuilder.Entity<BillingProduct>(entity =>
            {
                entity.ToTable("billing_products");
                entity.Property(bp => bp.Id).HasColumnName("id");
                entity.Property(bp => bp.Quantity).HasColumnName("quantity");
                entity.Property(bp => bp.TotalAmount).HasColumnName("total_amount");

                entity.HasOne(bp => bp.Product)  // Each BillingProduct has one Product
                      .WithMany(p => p.BillingProducts)  // A Product can be in many BillingProducts
                      .HasForeignKey(bp => bp.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);  // Restrict cascade deletion of products

                entity.HasOne(bp => bp.Billing)  // Each BillingProduct belongs to one Billing
                      .WithMany(b => b.BillingProducts)  // A Billing can have many BillingProducts
                      .HasForeignKey(bp => bp.BillingId)  // Foreign key pointing to Billing
                      .OnDelete(DeleteBehavior.Cascade);  // Cascade delete if Billing is deleted

                entity.Property(e => e.CreatedOn).HasColumnName(CommonNames.CreatedOn).HasDefaultValueSql(CommonNames.NOW);
                entity.Property(e => e.ModifiedOn).HasColumnName(CommonNames.ModifiedOn).HasDefaultValueSql(CommonNames.NOW);
                entity.Property(e => e.DataState).HasColumnName(CommonNames.DataState).HasDefaultValue(RecordState.Active);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
