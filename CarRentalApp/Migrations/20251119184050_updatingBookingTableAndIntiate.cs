using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class updatingBookingTableAndIntiate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                       

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    car_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pickup_location_id = table.Column<int>(type: "int", nullable: false),
                    dropoff_location_id = table.Column<int>(type: "int", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    pickup_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    return_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK_Bookings_Cars",
                        column: x => x.car_id,
                        principalTable: "Cars",
                        principalColumn: "car_id");
                    table.ForeignKey(
                        name: "FK_Bookings_DropoffLocation",
                        column: x => x.dropoff_location_id,
                        principalTable: "Locations",
                        principalColumn: "location_id");
                    table.ForeignKey(
                        name: "FK_Bookings_PickupLocation",
                        column: x => x.pickup_location_id,
                        principalTable: "Locations",
                        principalColumn: "location_id");
                    table.ForeignKey(
                        name: "FK_Bookings_Users",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    payment_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    payment_method = table.Column<int>(type: "int", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("payment_id", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.booking_id,
                        principalTable: "Bookings",
                        principalColumn: "booking_id");
                });

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "Payments");

            
            migrationBuilder.DropTable(
                name: "Bookings");

        }
    }
}
