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
    public class VacationRepository : IVacationRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public VacationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Vacation> AddVacationAsync(Vacation vacation)
        {
            _dbContext.Vacations.Add(vacation);
            await _dbContext.SaveChangesAsync();
            return vacation;
        }

        public async Task<bool> DeleteVacationByIdAsync(Guid id)
        {
            _dbContext.Vacations.RemoveRange(
                _dbContext.Vacations
                .Where(temp => temp.Id == id));

            int deletedRowsCount = await _dbContext.SaveChangesAsync();

            return deletedRowsCount > 0;
        }

        public async Task<List<Vacation>> GetAllVacationsAsync()
        {
            return await _dbContext.Vacations.ToListAsync();
        }

        public async Task<Vacation?> GetVacationByIdAsync(Guid id)
        {
            return await _dbContext.Vacations.FirstOrDefaultAsync(temp => temp.Id == id);
        }

        public async Task<Vacation> UpdateVacationAsync(Vacation vacation)
        {
            Vacation? vacationFromDb = await _dbContext.Vacations
                .FirstOrDefaultAsync(temp => temp.Id == vacation.Id);

            // in case no object is found in the database, return the parameter object
            if (vacationFromDb == null)
                return vacation;

            vacationFromDb.StartDate = vacation.StartDate;
            vacationFromDb.EndDate = vacation.EndDate;
            vacationFromDb.ApplicationUserId = vacation.ApplicationUserId;

            await _dbContext.SaveChangesAsync();

            return vacationFromDb;
        }

        public async Task<Dictionary<DateOnly, DateOnly>> GetVacationsDictionaryAsync(Guid? userId)
        {
            // new dictionary with DateOnly as key and string as value
            Dictionary<DateOnly, DateOnly> dictionaryToReturn = new Dictionary<DateOnly, DateOnly>();

            // get the list of all vacations of the user that has the given userId
            var vacationsFromGet = await _dbContext.Vacations.Where(vacation => vacation.ApplicationUserId == userId).ToListAsync();

            // if the list is empty, return the empty dictionary
            if (vacationsFromGet == null)
                return dictionaryToReturn;

            // for each vacation, if the vacation date is not null, add it to the dictionary
            foreach (var day in vacationsFromGet)
            {
                if (!(day.StartDate.Equals(null) || day.EndDate.Equals(null)))
                {
                    dictionaryToReturn[day.StartDate.Value] = day.EndDate.Value;
                }
            }

            return dictionaryToReturn;
        }
    }
}
