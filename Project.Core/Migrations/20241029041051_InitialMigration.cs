using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Project.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_name = table.Column<string>(type: "text", nullable: false),
                    category_code = table.Column<short>(type: "smallint", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    data_state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    user_pass = table.Column<string>(type: "text", nullable: false),
                    ConfirmPassword = table.Column<string>(type: "text", nullable: false),
                    email_id = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    data_state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(8129)", maxLength: 8129, nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    product_price = table.Column<decimal>(type: "numeric", nullable: false),
                    total_products = table.Column<int>(type: "integer", nullable: false),
                    category_fk_id = table.Column<long>(type: "bigint", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    data_state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_category_category_fk_id",
                        column: x => x.category_fk_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "billings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    billing_ref_no = table.Column<string>(type: "text", nullable: false),
                    billing_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    customer_fk = table.Column<long>(type: "bigint", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    data_state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billings", x => x.id);
                    table.ForeignKey(
                        name: "FK_billings_user_customer_fk",
                        column: x => x.customer_fk,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_fk_id = table.Column<long>(type: "bigint", nullable: false),
                    product_fk_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<long>(type: "bigint", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    data_state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart", x => x.id);
                    table.ForeignKey(
                        name: "FK_cart_products_product_fk_id",
                        column: x => x.product_fk_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_user_user_fk_id",
                        column: x => x.user_fk_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "billing_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    BillingId = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    data_state = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billing_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_billing_products_billings_BillingId",
                        column: x => x.BillingId,
                        principalTable: "billings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_billing_products_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_billing_products_BillingId",
                table: "billing_products",
                column: "BillingId");

            migrationBuilder.CreateIndex(
                name: "IX_billing_products_ProductId",
                table: "billing_products",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_billings_customer_fk",
                table: "billings",
                column: "customer_fk");

            migrationBuilder.CreateIndex(
                name: "IX_cart_product_fk_id",
                table: "cart",
                column: "product_fk_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_user_fk_id",
                table: "cart",
                column: "user_fk_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_fk_id",
                table: "products",
                column: "category_fk_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "billing_products");

            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "billings");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
