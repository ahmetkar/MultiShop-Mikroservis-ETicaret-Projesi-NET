using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiShop.Cargo.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class dbfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "CargoOperations");

            migrationBuilder.DropColumn(
                name: "ReceiverCustomer",
                table: "CargoDetails");

            migrationBuilder.DropColumn(
                name: "SenderCustomer",
                table: "CargoDetails");

            migrationBuilder.AddColumn<int>(
                name: "CargoDetailId",
                table: "CargoOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "CargoOperations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "CargoDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "CargoDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CargoOperations_CargoDetailId",
                table: "CargoOperations",
                column: "CargoDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CargoDetails_CustomerId",
                table: "CargoDetails",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CargoDetails_CargoCustomers_CustomerId",
                table: "CargoDetails",
                column: "CustomerId",
                principalTable: "CargoCustomers",
                principalColumn: "CargoCustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CargoOperations_CargoDetails_CargoDetailId",
                table: "CargoOperations",
                column: "CargoDetailId",
                principalTable: "CargoDetails",
                principalColumn: "CargoDetailId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoDetails_CargoCustomers_CustomerId",
                table: "CargoDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CargoOperations_CargoDetails_CargoDetailId",
                table: "CargoOperations");

            migrationBuilder.DropIndex(
                name: "IX_CargoOperations_CargoDetailId",
                table: "CargoOperations");

            migrationBuilder.DropIndex(
                name: "IX_CargoDetails_CustomerId",
                table: "CargoDetails");

            migrationBuilder.DropColumn(
                name: "CargoDetailId",
                table: "CargoOperations");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "CargoOperations");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CargoDetails");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "CargoOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Barcode",
                table: "CargoDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverCustomer",
                table: "CargoDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderCustomer",
                table: "CargoDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
