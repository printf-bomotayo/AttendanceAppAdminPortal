using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "204fdfcb-ac86-48a9-8d80-e5d5d889b378");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a94d842c-8428-47aa-90c2-d3cae7afbd43");

            migrationBuilder.RenameColumn(
                name: "CandidateGender",
                table: "Candidates",
                newName: "Gender");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "bdd87c02-487d-4760-9fb4-a938ecbdf3a7", null, "Member", "MEMBER" },
                    { "f1eb1b37-3fc8-4ea5-8870-5ce05c59427e", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bdd87c02-487d-4760-9fb4-a938ecbdf3a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1eb1b37-3fc8-4ea5-8870-5ce05c59427e");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Candidates",
                newName: "CandidateGender");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "204fdfcb-ac86-48a9-8d80-e5d5d889b378", null, "Admin", "ADMIN" },
                    { "a94d842c-8428-47aa-90c2-d3cae7afbd43", null, "Member", "MEMBER" }
                });
        }
    }
}
