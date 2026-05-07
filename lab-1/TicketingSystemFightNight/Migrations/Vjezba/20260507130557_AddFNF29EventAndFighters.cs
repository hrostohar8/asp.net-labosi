using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketingSystemFightNight.Migrations.Vjezba
{
    /// <inheritdoc />
    public partial class AddFNF29EventAndFighters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Arenas",
                columns: new[] { "Id", "Address", "Capacity", "City", "IsIndoor", "Name", "OpenedYear" },
                values: new object[] { 4, "Štefanova cesta 6", 14000, "Ljubljana", true, "Ljubljana Arena", 2002 });

            migrationBuilder.InsertData(
                table: "Fighters",
                columns: new[] { "Id", "Country", "Losses", "Name", "Nickname", "Organization", "WeightClass", "Wins" },
                values: new object[,]
                {
                    { 7, "Slovenia", 2, "Matej Batinić", "The Slovenian Storm", 2, 5, 18 },
                    { 8, "Slovenia", 3, "Jakob Nedoh", "The Ljubljana Lion", 2, 5, 17 }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "BaseTicketPrice", "City", "Date", "Description", "Name", "Organization", "TicketsSold", "Time", "VenueId" },
                values: new object[] { 4, 220m, "Ljubljana", new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main event: Batinić vs Nedoh", "FNF 29 Ljubljana", 2, 5600, new TimeSpan(0, 19, 0, 0, 0), 4 });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "Championship", "EventId", "Fighter1Id", "Fighter2Id", "Referee", "RoundLimit", "Status", "WeightClass" },
                values: new object[] { 4, false, 4, 7, 8, "Slavko Kosanović", 5, "Scheduled", 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Arenas",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
