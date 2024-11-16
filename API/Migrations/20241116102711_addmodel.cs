using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class addmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TrainingPrograms",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TrainingPrograms",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TrainingPrograms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TrainingPrograms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "TrainingPrograms",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "TrainingPrograms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "TrainingPrograms",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "TotalTrainingDays",
                table: "TrainingPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Cohorts",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "TotalTrainingDays",
                table: "TrainingPrograms");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TrainingPrograms",
                newName: "Title");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Cohorts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
