using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85eeee46-fbc8-417e-86aa-a273559c8979");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae90168d-8c4a-44c3-8ba4-1bc2772978c2");

            migrationBuilder.AddColumn<int>(
                name: "CandidateGender",
                table: "Candidates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Candidates",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StaffId",
                table: "Candidates",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "AttendanceRecords",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AttendanceRecords",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "AttendanceRecords",
                type: "REAL",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TrainingPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    FacilitatingInstitution = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationUnit = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cohorts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    TrainingProgramId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cohorts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cohorts_TrainingPrograms_TrainingProgramId",
                        column: x => x.TrainingProgramId,
                        principalTable: "TrainingPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "204fdfcb-ac86-48a9-8d80-e5d5d889b378", null, "Admin", "ADMIN" },
                    { "a94d842c-8428-47aa-90c2-d3cae7afbd43", null, "Member", "MEMBER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_CohortId",
                table: "Candidates",
                column: "CohortId");

            migrationBuilder.CreateIndex(
                name: "IX_Cohorts_TrainingProgramId",
                table: "Cohorts",
                column: "TrainingProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Cohorts_CohortId",
                table: "Candidates",
                column: "CohortId",
                principalTable: "Cohorts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Cohorts_CohortId",
                table: "Candidates");

            migrationBuilder.DropTable(
                name: "Cohorts");

            migrationBuilder.DropTable(
                name: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_CohortId",
                table: "Candidates");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "204fdfcb-ac86-48a9-8d80-e5d5d889b378");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a94d842c-8428-47aa-90c2-d3cae7afbd43");

            migrationBuilder.DropColumn(
                name: "CandidateGender",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AttendanceRecords");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "85eeee46-fbc8-417e-86aa-a273559c8979", null, "Member", "MEMBER" },
                    { "ae90168d-8c4a-44c3-8ba4-1bc2772978c2", null, "Admin", "ADMIN" }
                });
        }
    }
}
