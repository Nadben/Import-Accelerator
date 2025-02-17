using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Accelerator.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _01_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category_Group_Subgroup_Partterm_mappingGenerated",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Groupid = table.Column<int>(type: "integer", nullable: false),
                    Subgroupid = table.Column<int>(type: "integer", nullable: false),
                    Parttermid = table.Column<int>(type: "integer", nullable: false),
                    Groupname = table.Column<string>(type: "text", nullable: true),
                    Subgroupname = table.Column<string>(type: "text", nullable: true),
                    Parttermname = table.Column<string>(type: "text", nullable: true),
                    USG = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category_Group_Subgroup_Partterm_mappingGenerated", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventory_USIC_INV20240325152337Generated",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mfg = table.Column<string>(type: "text", nullable: true),
                    CurrentSubline = table.Column<int>(type: "integer", nullable: false),
                    Part = table.Column<string>(type: "text", nullable: true),
                    StdUPC = table.Column<double>(type: "double precision", nullable: false),
                    LocationID = table.Column<int>(type: "integer", nullable: false),
                    StockQty = table.Column<int>(type: "integer", nullable: false),
                    QtyMinOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory_USIC_INV20240325152337Generated", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Price_USIC_PRICE20240325152337ABGenerated",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mfg = table.Column<string>(type: "text", nullable: true),
                    Subline = table.Column<int>(type: "integer", nullable: false),
                    Part = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Core = table.Column<double>(type: "double precision", nullable: false),
                    Ehfee = table.Column<double>(type: "double precision", nullable: false),
                    Regionid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price_USIC_PRICE20240325152337ABGenerated", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Price_USIC_PRICE20240325152337BCGenerated",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mfg = table.Column<string>(type: "text", nullable: true),
                    Subline = table.Column<int>(type: "integer", nullable: false),
                    Part = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Core = table.Column<double>(type: "double precision", nullable: false),
                    Ehfee = table.Column<double>(type: "double precision", nullable: false),
                    Regionid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price_USIC_PRICE20240325152337BCGenerated", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category_Group_Subgroup_Partterm_mappingGenerated");

            migrationBuilder.DropTable(
                name: "Inventory_USIC_INV20240325152337Generated");

            migrationBuilder.DropTable(
                name: "Price_USIC_PRICE20240325152337ABGenerated");

            migrationBuilder.DropTable(
                name: "Price_USIC_PRICE20240325152337BCGenerated");
        }
    }
}
