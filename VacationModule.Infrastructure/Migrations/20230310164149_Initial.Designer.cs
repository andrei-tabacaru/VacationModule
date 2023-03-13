﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VacationModule.Infrastructure.Context;

#nullable disable

namespace VacationModule.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230310164149_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VacationModule.Core.Domain.Entities.NationalHoliday", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly?>("HolidayDate")
                        .HasColumnType("date");

                    b.Property<string>("HolidayName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("NationalHolidays");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c71c9bcf-3747-40a2-b005-b63f95c6f8db"),
                            HolidayDate = new DateOnly(2023, 1, 1),
                            HolidayName = "New Year's Day"
                        },
                        new
                        {
                            Id = new Guid("9cde8a3c-df02-4bae-9ace-8ada36363b38"),
                            HolidayDate = new DateOnly(2023, 1, 2),
                            HolidayName = "Day after New Year's Day"
                        },
                        new
                        {
                            Id = new Guid("dc1b5e58-30b2-4dea-bca5-166bc9e799df"),
                            HolidayDate = new DateOnly(2023, 1, 24),
                            HolidayName = "Unification Day"
                        },
                        new
                        {
                            Id = new Guid("65655954-832b-4a51-af7f-8ba1efdd20aa"),
                            HolidayDate = new DateOnly(2023, 4, 22),
                            HolidayName = "Orthodox Good FrIday"
                        },
                        new
                        {
                            Id = new Guid("74835416-7473-47e8-8d3d-c53d12666895"),
                            HolidayDate = new DateOnly(2023, 4, 24),
                            HolidayName = "Orthodox Easter Day"
                        },
                        new
                        {
                            Id = new Guid("0b6cb8cd-86cb-4c37-a292-f5e05a484ed2"),
                            HolidayDate = new DateOnly(2023, 4, 25),
                            HolidayName = "Orthodox Easter Monday"
                        },
                        new
                        {
                            Id = new Guid("3a21493d-82ce-4b2f-b33f-e806577546a1"),
                            HolidayDate = new DateOnly(2023, 5, 1),
                            HolidayName = "Labour Day"
                        },
                        new
                        {
                            Id = new Guid("7ee6f713-389d-437f-9543-b73cdeefd91a"),
                            HolidayDate = new DateOnly(2023, 6, 1),
                            HolidayName = "International Children's Day"
                        },
                        new
                        {
                            Id = new Guid("260f0bb1-59d3-401f-ad89-ed691af17eef"),
                            HolidayDate = new DateOnly(2023, 6, 12),
                            HolidayName = "Orthodox Pentecost"
                        },
                        new
                        {
                            Id = new Guid("2136abe1-9921-441b-b9c0-176e2972e48d"),
                            HolidayDate = new DateOnly(2023, 6, 13),
                            HolidayName = "Descent the of Holy Spirit"
                        },
                        new
                        {
                            Id = new Guid("b0568024-46d2-4e5d-a61f-08ccc2acee90"),
                            HolidayDate = new DateOnly(2023, 8, 15),
                            HolidayName = "St Mary's Day"
                        },
                        new
                        {
                            Id = new Guid("25f590a2-2d93-4b59-9481-9f95d9adb790"),
                            HolidayDate = new DateOnly(2023, 11, 30),
                            HolidayName = "Feast of St. Andrew"
                        },
                        new
                        {
                            Id = new Guid("d40c8731-1bf5-4729-867b-b9d2fe8f2e97"),
                            HolidayDate = new DateOnly(2023, 12, 1),
                            HolidayName = "National Day"
                        },
                        new
                        {
                            Id = new Guid("d40c8731-1bf5-4729-867b-b9d2ce8f2e97"),
                            HolidayDate = new DateOnly(2023, 12, 25),
                            HolidayName = "Christmas Day"
                        },
                        new
                        {
                            Id = new Guid("d40c8731-1bf5-4729-867b-b9d2ce8f2e98"),
                            HolidayDate = new DateOnly(2023, 12, 26),
                            HolidayName = "Second day of Christmas"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
