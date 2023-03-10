using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.Domain.RepositoryContracts;
using VacationModule.Infrastructure.Context;

namespace VacationModule.Infrastructure.Repositories
{
    public class NationalHolidayRepository : INationalHolidayRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public NationalHolidayRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<NationalHoliday> AddNationalHolidayAsync(NationalHoliday nationalHoliday)
        {
            _dbContext.NationalHolidays.Add(nationalHoliday);
            await _dbContext.SaveChangesAsync();
            return nationalHoliday;
        }

        public async Task<bool> DeleteNationalHolidayByIdAsync(Guid id)
        {
            _dbContext.NationalHolidays.RemoveRange(
                _dbContext.NationalHolidays
                .Where(temp => temp.Id == id));

            int deletedRowsCount = await _dbContext.SaveChangesAsync();

            return deletedRowsCount > 0;
        }

        public async Task<List<NationalHoliday>> GetAllNationalHolidaysAsync()
        {
            return await _dbContext.NationalHolidays.ToListAsync();
        }

        public async Task<NationalHoliday?> GetNationalHolidayByIdAsync(Guid id)
        {
            return await _dbContext.NationalHolidays.FirstOrDefaultAsync(temp => temp.Id == id);
        }

        public async Task<NationalHoliday> UpdateNationalHolidayAsync(NationalHoliday nationalHoliday)
        {
            NationalHoliday? nationalHolidayFromDb = await _dbContext.NationalHolidays
                .FirstOrDefaultAsync(temp => temp.Id == nationalHoliday.Id);

            // in case no object is found in the database, return the parameter object
            if (nationalHolidayFromDb == null)
                return nationalHoliday;

            nationalHolidayFromDb.HolidayDate = nationalHoliday.HolidayDate;
            nationalHolidayFromDb.HolidayName = nationalHoliday.HolidayName;

            await _dbContext.SaveChangesAsync();

            return nationalHolidayFromDb;
        }
    }
}
