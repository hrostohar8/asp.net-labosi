using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketingSystemFightNight.Migrations.Vjezba
{
    /// <inheritdoc />
    public partial class SyncVjezbaSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeightClass",
                table: "Matches",
                newName: "WeightClassId");

            migrationBuilder.RenameColumn(
                name: "WeightClass",
                table: "Fighters",
                newName: "WeightClassId");

            migrationBuilder.RenameColumn(
                name: "Organization",
                table: "Fighters",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "Organization",
                table: "Events",
                newName: "OrganizationId");

            migrationBuilder.CreateTable(
                name: "FightOrganizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FightOrganizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketShopModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    EventName = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Section = table.Column<string>(type: "TEXT", nullable: false),
                    Row = table.Column<int>(type: "INTEGER", nullable: false),
                    Seat = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsVip = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketShopModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeightClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightClasses", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Arenas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8206));

            migrationBuilder.UpdateData(
                table: "Arenas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8210));

            migrationBuilder.UpdateData(
                table: "Arenas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8213));

            migrationBuilder.UpdateData(
                table: "Arenas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8216));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "OrganizationId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8263), 1 });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "OrganizationId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8286), 1 });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "OrganizationId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8292), 1 });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "OrganizationId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8298), 3 });

            migrationBuilder.InsertData(
                table: "FightOrganizations",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8147), null, "UFC", null },
                    { 2, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8157), null, "KSW", null },
                    { 3, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8160), null, "FNC", null },
                    { 4, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8162), null, "BELLATOR", null },
                    { 5, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8166), null, "ONE FC", null }
                });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "OrganizationId", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8220), 1, 7 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "OrganizationId", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8229), 1, 8 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "OrganizationId", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8234), 1, 7 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "OrganizationId", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8238), 1, 7 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "OrganizationId", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8242), 1, 8 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "OrganizationId", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8246), 1, 2 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "OrganizationId", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8250), 3, 6 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "OrganizationId", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8254), 3, 6 });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8306), 7 });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8319), 8 });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8324), 7 });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "WeightClassId" },
                values: new object[] { new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8328), 6 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8337));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8347));

            migrationBuilder.InsertData(
                table: "WeightClasses",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8174), null, "Flyweight", null },
                    { 2, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8180), null, "Bantamweight", null },
                    { 3, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8182), null, "Featherweight", null },
                    { 4, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8185), null, "Lightweight", null },
                    { 5, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8187), null, "Welterweight", null },
                    { 6, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8190), null, "Middleweight", null },
                    { 7, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8192), null, "LightHeavyweight", null },
                    { 8, new DateTime(2026, 5, 18, 10, 30, 29, 709, DateTimeKind.Utc).AddTicks(8195), null, "Heavyweight", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_WeightClassId",
                table: "Matches",
                column: "WeightClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Fighters_OrganizationId",
                table: "Fighters",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Fighters_WeightClassId",
                table: "Fighters",
                column: "WeightClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizationId",
                table: "Events",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_FightOrganizations_OrganizationId",
                table: "Events",
                column: "OrganizationId",
                principalTable: "FightOrganizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighters_FightOrganizations_OrganizationId",
                table: "Fighters",
                column: "OrganizationId",
                principalTable: "FightOrganizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fighters_WeightClasses_WeightClassId",
                table: "Fighters",
                column: "WeightClassId",
                principalTable: "WeightClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_WeightClasses_WeightClassId",
                table: "Matches",
                column: "WeightClassId",
                principalTable: "WeightClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_FightOrganizations_OrganizationId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighters_FightOrganizations_OrganizationId",
                table: "Fighters");

            migrationBuilder.DropForeignKey(
                name: "FK_Fighters_WeightClasses_WeightClassId",
                table: "Fighters");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_WeightClasses_WeightClassId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "FightOrganizations");

            migrationBuilder.DropTable(
                name: "TicketShopModels");

            migrationBuilder.DropTable(
                name: "WeightClasses");

            migrationBuilder.DropIndex(
                name: "IX_Matches_WeightClassId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Fighters_OrganizationId",
                table: "Fighters");

            migrationBuilder.DropIndex(
                name: "IX_Fighters_WeightClassId",
                table: "Fighters");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganizationId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "WeightClassId",
                table: "Matches",
                newName: "WeightClass");

            migrationBuilder.RenameColumn(
                name: "WeightClassId",
                table: "Fighters",
                newName: "WeightClass");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Fighters",
                newName: "Organization");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Events",
                newName: "Organization");

            migrationBuilder.UpdateData(
                table: "Arenas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "Arenas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6423));

            migrationBuilder.UpdateData(
                table: "Arenas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6427));

            migrationBuilder.UpdateData(
                table: "Arenas",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6430));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Organization" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6462), 0 });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Organization" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6487), 0 });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Organization" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6492), 0 });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Organization" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6497), 2 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Organization", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6435), 0, 6 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Organization", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6444), 0, 7 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Organization", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6446), 0, 6 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Organization", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6448), 0, 6 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Organization", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6450), 0, 7 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Organization", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6452), 0, 1 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Organization", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6454), 2, 5 });

            migrationBuilder.UpdateData(
                table: "Fighters",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "Organization", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6456), 2, 5 });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6505), 6 });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6514), 7 });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6517), 6 });

            migrationBuilder.UpdateData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "WeightClass" },
                values: new object[] { new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6521), 5 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6530));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 17, 15, 48, 40, 948, DateTimeKind.Utc).AddTicks(6540));
        }
    }
}
