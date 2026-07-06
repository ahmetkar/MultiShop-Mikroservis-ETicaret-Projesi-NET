using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiShop.Cargo.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class dbfix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "CargoCustomers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "CargoCustomers");

            migrationBuilder.DropColumn(
                name: "District",
                table: "CargoCustomers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "CargoCustomers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CargoCustomers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "CargoCustomers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "CargoCustomers");

            migrationBuilder.AddColumn<int>(
                name: "OrderingId",
                table: "CargoOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderingId",
                table: "CargoOperations");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "CargoCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "CargoCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "CargoCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CargoCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CargoCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "CargoCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "CargoCustomers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
