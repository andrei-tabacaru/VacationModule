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
    public class NationalHolidayUpdateRepository : INationalHolidayUpdateRepository
    {
        private readonly NationalHolidayRepository _nationalHolidayRepository;
        public NationalHolidayUpdateRepository(NationalHolidayRepository nationalHolidayRepository)
        {
            _nationalHolidayRepository = nationalHolidayRepository;
        }
        public async Task<NationalHoliday> AddNationalHolidayAsync(NationalHoliday nationalHoliday)
        {
            return await _nationalHolidayRepository.AddNationalHolidayAsync(nationalHoliday);
        }

        public async Task<bool> DeleteNationalHolidayByIdAsync(Guid id)
        {
            return await _nationalHolidayRepository.DeleteNationalHolidayByIdAsync(id);
        }

        public async Task<List<NationalHoliday>> GetAllNationalHolidaysAsync()
        {
            return await _nationalHolidayRepository.GetAllNationalHolidaysAsync();
        }

        public async Task<NationalHoliday?> GetNationalHolidayByIdAsync(Guid id)
        {
            return await _nationalHolidayRepository.GetNationalHolidayByIdAsync(id);
        }

        public async Task<NationalHoliday> UpdateNationalHolidayAsync(NationalHoliday nationalHoliday)
        {
            return await _nationalHolidayRepository.UpdateNationalHolidayAsync(nationalHoliday);
        }

        public async Task<Dictionary<DateOnly, string?>> GetNationalHolidaysDictionaryAsync()
        {
            return await _nationalHolidayRepository.GetNationalHolidaysDictionaryAsync();
        }

        public async Task<Dictionary<DateOnly, string?>> GetNationalHolidaysDictionaryYearAsync(int? year)
        {
            // year can' be null or negative value
            if(year == null || year < 0)
            {
                throw new ArgumentNullException(nameof(year));
            }

            // new dictionary with DateOnly as key and string as value
            Dictionary<DateOnly, string?> dictionaryToReturn = new Dictionary<DateOnly, string?>();

            // get the list of all national holidays
            var nationalHolidaysFromGet = await _nationalHolidayRepository.GetAllNationalHolidaysAsync();

            // if the list is empty, return the empty dictionary
            if (nationalHolidaysFromGet == null)
                return dictionaryToReturn;

            // for each holiday, if the holiday date is not null, add it to the dictionary
            foreach (var day in nationalHolidaysFromGet)
            {
                if (!day.HolidayDate.Equals(null))
                {
                    // replace the year with the year from input
                    DateOnly newDate = new DateOnly((int)year, day.HolidayDate.Value.Month, day.HolidayDate.Value.Day);
                    dictionaryToReturn[newDate] = day.HolidayName;
                }
            }

            return dictionaryToReturn;
        }
    }
}
