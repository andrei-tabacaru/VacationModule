﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.Domain.IdentityEntities;

namespace VacationModule.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<NationalHoliday> NationalHolidays { get; set;} 
        public virtual DbSet<Vacation> Vacations { get; set;} 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed to NationalHolidays
            // Get Json file to string
            string nationalHolidaysJson = System.IO.File.ReadAllText("nationalholidays.json");

            // Transform string to list of objects
            List<NationalHoliday> nationalHolidays = System.Text.Json.JsonSerializer
                .Deserialize<List<NationalHoliday>>(nationalHolidaysJson);

            // For each object, add it in the table
            foreach (NationalHoliday nationalHoliday in nationalHolidays)
            {
                modelBuilder.Entity<NationalHoliday>().HasData(nationalHoliday);
            }
        }
    }
}
