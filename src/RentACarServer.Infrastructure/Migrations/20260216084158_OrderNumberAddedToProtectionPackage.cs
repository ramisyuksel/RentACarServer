using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderNumberAddedToProtectionPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNumber_Value",
                table: "ProtectionPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber_Value",
                table: "ProtectionPackages");
        }
    }
}
