using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketingSystemFightNight.Migrations.Vjezba
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arenas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    IsIndoor = table.Column<bool>(type: "INTEGER", nullable: false),
                    OpenedYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arenas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fighters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Nickname = table.Column<string>(type: "TEXT", nullable: false),
                    WeightClass = table.Column<int>(type: "INTEGER", nullable: false),
                    Organization = table.Column<int>(type: "INTEGER", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Wins = table.Column<int>(type: "INTEGER", nullable: false),
                    Losses = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fighters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LoyaltyPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    IsVip = table.Column<bool>(type: "INTEGER", nullable: false),
                    MemberLevel = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Organization = table.Column<int>(type: "INTEGER", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Time = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    VenueId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    BaseTicketPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    TicketsSold = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Arenas_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Arenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiscountCode = table.Column<string>(type: "TEXT", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fighter1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Fighter2Id = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightClass = table.Column<int>(type: "INTEGER", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoundLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    Championship = table.Column<bool>(type: "INTEGER", nullable: false),
                    Referee = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Fighters_Fighter1Id",
                        column: x => x.Fighter1Id,
                        principalTable: "Fighters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Fighters_Fighter2Id",
                        column: x => x.Fighter2Id,
                        principalTable: "Fighters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    CartId = table.Column<int>(type: "INTEGER", nullable: true),
                    Section = table.Column<string>(type: "TEXT", nullable: false),
                    Row = table.Column<int>(type: "INTEGER", nullable: false),
                    Seat = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsVip = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Arenas",
                columns: new[] { "Id", "Address", "Capacity", "City", "IsIndoor", "Name", "OpenedYear" },
                values: new object[,]
                {
                    { 1, "Ulica Grada Vukovara 269a", 15000, "Zagreb", true, "Arena Zagreb", 2008 },
                    { 2, "Ulica Domovinskog rata 2", 12000, "Split", false, "Arena Split", 2019 },
                    { 3, "Trg Riječke rezolucije 2", 10000, "Rijeka", true, "Arena Rijeka", 2014 }
                });

            migrationBuilder.InsertData(
                table: "Fighters",
                columns: new[] { "Id", "Country", "Losses", "Name", "Nickname", "Organization", "WeightClass", "Wins" },
                values: new object[,]
                {
                    { 1, "USA", 1, "Jon Jones", "Bones", 0, 6, 26 },
                    { 2, "Cameroon", 3, "Francis Ngannou", "The Predator", 0, 7, 16 },
                    { 3, "Sweden", 7, "Alexander Gustafsson", "The Mauler", 0, 6, 18 },
                    { 4, "USA", 3, "Daniel Cormier", "DC", 0, 6, 22 },
                    { 5, "USA", 4, "Stipe Miocic", "The Croatian Sensation", 0, 7, 20 },
                    { 6, "Brazil", 5, "Amanda Nunes", "The Lioness", 0, 1, 21 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "Email", "IsVip", "LoyaltyPoints", "MemberLevel", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", true, 150, "Administrator", "admin", "+385911234567" },
                    { 2, new DateTime(1988, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", false, 200, "Regular User", "john_doe", "+385912345678" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "BaseTicketPrice", "City", "Date", "Description", "Name", "Organization", "TicketsSold", "Time", "VenueId" },
                values: new object[,]
                {
                    { 1, 250m, "Zagreb", new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main event: Jones vs Gustafsson", "UFC Fight Night Zagreb", 0, 12850, new TimeSpan(0, 19, 0, 0, 0), 1 },
                    { 2, 180m, "Split", new DateTime(2024, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main event: Ngannou vs Miocic", "UFC Fight Night Split", 0, 9800, new TimeSpan(0, 19, 0, 0, 0), 2 },
                    { 3, 150m, "Rijeka", new DateTime(2024, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main event: Cormier vs Gustafsson", "UFC Fight Night Rijeka", 0, 7600, new TimeSpan(0, 19, 0, 0, 0), 3 }
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "Championship", "EventId", "Fighter1Id", "Fighter2Id", "Referee", "RoundLimit", "Status", "WeightClass" },
                values: new object[,]
                {
                    { 1, true, 1, 1, 3, "Herb Dean", 5, "Scheduled", 6 },
                    { 2, false, 2, 2, 5, "John McCarthy", 5, "Scheduled", 7 },
                    { 3, false, 3, 4, 3, "Yves Lavigne", 5, "Scheduled", 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_VenueId",
                table: "Events",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_EventId",
                table: "Matches",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Fighter1Id",
                table: "Matches",
                column: "Fighter1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Fighter2Id",
                table: "Matches",
                column: "Fighter2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CartId",
                table: "Tickets",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Fighters");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Arenas");
        }
    }
}
