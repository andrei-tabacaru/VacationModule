using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VacationModule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NationalHolidays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HolidayName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    HolidayDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NationalHolidays", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NationalHolidays",
                columns: new[] { "Id", "HolidayDate", "HolidayName" },
                values: new object[,]
                {
                    { new Guid("0b6cb8cd-86cb-4c37-a292-f5e05a484ed2"), new DateOnly(2023, 4, 25), "Orthodox Easter Monday" },
                    { new Guid("2136abe1-9921-441b-b9c0-176e2972e48d"), new DateOnly(2023, 6, 13), "Descent the of Holy Spirit" },
                    { new Guid("25f590a2-2d93-4b59-9481-9f95d9adb790"), new DateOnly(2023, 11, 30), "Feast of St. Andrew" },
                    { new Guid("260f0bb1-59d3-401f-ad89-ed691af17eef"), new DateOnly(2023, 6, 12), "Orthodox Pentecost" },
                    { new Guid("3a21493d-82ce-4b2f-b33f-e806577546a1"), new DateOnly(2023, 5, 1), "Labour Day" },
                    { new Guid("65655954-832b-4a51-af7f-8ba1efdd20aa"), new DateOnly(2023, 4, 22), "Orthodox Good FrIday" },
                    { new Guid("74835416-7473-47e8-8d3d-c53d12666895"), new DateOnly(2023, 4, 24), "Orthodox Easter Day" },
                    { new Guid("7ee6f713-389d-437f-9543-b73cdeefd91a"), new DateOnly(2023, 6, 1), "International Children's Day" },
                    { new Guid("9cde8a3c-df02-4bae-9ace-8ada36363b38"), new DateOnly(2023, 1, 2), "Day after New Year's Day" },
                    { new Guid("b0568024-46d2-4e5d-a61f-08ccc2acee90"), new DateOnly(2023, 8, 15), "St Mary's Day" },
                    { new Guid("c71c9bcf-3747-40a2-b005-b63f95c6f8db"), new DateOnly(2023, 1, 1), "New Year's Day" },
                    { new Guid("d40c8731-1bf5-4729-867b-b9d2ce8f2e97"), new DateOnly(2023, 12, 25), "Christmas Day" },
                    { new Guid("d40c8731-1bf5-4729-867b-b9d2ce8f2e98"), new DateOnly(2023, 12, 26), "Second day of Christmas" },
                    { new Guid("d40c8731-1bf5-4729-867b-b9d2fe8f2e97"), new DateOnly(2023, 12, 1), "National Day" },
                    { new Guid("dc1b5e58-30b2-4dea-bca5-166bc9e799df"), new DateOnly(2023, 1, 24), "Unification Day" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NationalHolidays");
        }
    }
}
