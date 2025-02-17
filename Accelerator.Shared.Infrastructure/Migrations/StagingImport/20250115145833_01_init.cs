using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accelerator.Shared.Infrastructure.Migrations.StagingImport
{
    /// <inheritdoc />
    public partial class _01_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommercetoolsCategoryImports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Hash = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PreviousHash = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercetoolsCategoryImports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommercetoolsInventoryImports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Sku = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    QuantityOnStock = table.Column<int>(type: "integer", nullable: false),
                    SupplyChannel = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Custom = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Hash = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PreviousHash = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercetoolsInventoryImports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommercetoolsStandalonePrice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Sku = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Value = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Channel = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Custom = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Hash = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PreviousHash = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercetoolsStandalonePrice", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommercetoolsCategoryImports");

            migrationBuilder.DropTable(
                name: "CommercetoolsInventoryImports");

            migrationBuilder.DropTable(
                name: "CommercetoolsStandalonePrice");
        }
    }
}
