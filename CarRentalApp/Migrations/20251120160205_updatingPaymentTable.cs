using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class updatingPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Bookings_BookingId",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "payment_status",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "transaction_id",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Bookings_BookingId",
                table: "Payments",
                column: "booking_id",
                principalTable: "Bookings",
                principalColumn: "booking_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Bookings_BookingId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "payment_status",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "transaction_id",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Bookings_BookingId",
                table: "Payments",
                column: "booking_id",
                principalTable: "Bookings",
                principalColumn: "booking_id");
        }
    }
}
