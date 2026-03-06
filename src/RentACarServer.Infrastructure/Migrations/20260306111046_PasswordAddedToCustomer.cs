using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PasswordAddedToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Password_PasswordHash",
                table: "Customers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Password_PasswordSalt",
                table: "Customers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password_PasswordHash",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Password_PasswordSalt",
                table: "Customers");
        }
    }
}
