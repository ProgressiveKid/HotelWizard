using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelWizard.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DateBookeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateBookeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: false),
                    CountDay = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Guests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomImages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomImages", x => x.id);
                    table.ForeignKey(
                        name: "FK_RoomImages_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DateBookeds",
                columns: new[] { "Id", "RoomId", "endDate", "startDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, new DateTime(2023, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Description", "Number", "PricePerNight", "Type" },
                values: new object[,]
                {
                    { 1, "Стандартный люкс с бассеином", "101", 100.0m, "I" },
                    { 2, "Обычный номер", "201", 50.0m, "II" },
                    { 3, "Обычный номер", "202", 50.0m, "III" }
                });

            migrationBuilder.InsertData(
                table: "RoomImages",
                columns: new[] { "id", "RoomId", "image" },
                values: new object[,]
                {
                    { 1, 1, "/images/Rooms/1/1.jpeg" },
                    { 2, 1, "/images/Rooms/1/2.jpeg" },
                    { 3, 1, "/images/Rooms/1/3.jpeg" },
                    { 4, 1, "/images/Rooms/1/4.jpeg" },
                    { 5, 2, "/images/Rooms/2/1.jpeg" },
                    { 6, 2, "/images/Rooms/2/2.jpeg" },
                    { 7, 2, "/images/Rooms/2/3.jpeg" },
                    { 8, 2, "/images/Rooms/2/4.jpeg" },
                    { 9, 3, "/images/Rooms/3/1.jpeg" },
                    { 10, 3, "/images/Rooms/3/2.jpeg" },
                    { 11, 3, "/images/Rooms/3/3.jpeg" },
                    { 12, 3, "/images/Rooms/3/4.jpeg" },
                    { 13, 3, "/images/Rooms/3/5.jpeg" },
                    { 14, 3, "/images/Rooms/3/6.jpeg" },
                    { 15, 3, "/images/Rooms/3/7.jpeg" },
                    { 16, 3, "/images/Rooms/3/8.jpeg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_GuestId",
                table: "Reservations",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RoomId",
                table: "Reservations",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomImages_RoomId",
                table: "RoomImages",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DateBookeds");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "RoomImages");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
