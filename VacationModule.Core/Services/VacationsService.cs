
using VacationModule.Core.DTO;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.Domain.RepositoryContracts;
using VacationModule.Core.ServiceContracts;
using VacationModule.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VacationModule.Core.StaticDetails;
using System.Collections.Generic;

namespace VacationModule.Core.Services
{
    public class VacationsService : IVacationsService
    {
        private readonly IVacationRepository _vacationRepository;
        // National holiday repository is needed to GetNationalHolidaysDictionaryAsync method
        // The mentioned method return a dictionary with HolidayDate key and HolidayName value 
        // It is used to check efficeiently if a date is a national holiday
        private readonly INationalHolidayRepository _nationalHolidaysRepository;

        public VacationsService(IVacationRepository vacationRepository,
            INationalHolidayRepository nationalHolidayRepository)
        {

            _vacationRepository = vacationRepository;
            _nationalHolidaysRepository = nationalHolidayRepository;
        }
        public async Task<VacationResponse> AddVacationAsync(VacationAddRequest? vacationAddRequest, Guid? UserId)
        {
            // vacationAddRequest is null
            if(vacationAddRequest == null)
            {
                throw new ArgumentNullException(nameof(vacationAddRequest));
            }

            // StartDate is null
            if(vacationAddRequest.StartDate.Equals(null)) 
            {
                throw new ArgumentException(nameof(vacationAddRequest.StartDate));
            }

            // EndDate is null
            if (vacationAddRequest.EndDate.Equals(null))
            {
                throw new ArgumentException(nameof(vacationAddRequest.EndDate));
            }

            // EndDate < StartDate
            if(vacationAddRequest.EndDate < vacationAddRequest.StartDate)
            {
                throw new ArgumentException("EndDate can't be before StartDate");
            }

            // Given UserId is null
            if(UserId == null)
            {
                throw new ArgumentNullException(nameof(UserId));
            }

            // Check if the requests dates intersect with existing requests
            bool datesIntersect = await CheckIfDatesIntersect(vacationAddRequest.StartDate, vacationAddRequest.EndDate, UserId);
            if(datesIntersect)
            {
                throw new ArgumentException("There is an existing vacation request that intersects with the current request");
            }

            // Check if the request exceds the remaining available vacation days
            bool excedsAvailableVacationDays = await CheckIfRequestExcedsMaxDaysNumber(vacationAddRequest.StartDate, vacationAddRequest.EndDate, UserId);
            if (excedsAvailableVacationDays)
            {
                throw new ArgumentException("Not enough vacation days available");
            }

            // Convert object from VacationAddRequest to Vacation type
            Vacation vacation = vacationAddRequest.toVacation();

            // Generate Id
            vacation.Id = Guid.NewGuid();
            // Add user's Id
            vacation.ApplicationUserId = (Guid)UserId;

            // Add Vacation 
            await _vacationRepository.AddVacationAsync(vacation);

            return vacation.toVacationResponse();
        }

        public async Task<List<VacationResponse>> GetAllVacationsAsync(bool current = true, bool history = false, Guid? UserId = null)
        {
            if (UserId == null)
            {
                if(current == true && history == false)
                {
                    DateOnly currentDate = DateOnly.Parse(DateTime.Now.Date.ToString().Split(' ')[0]);
                    // Get all vacations
                    return (await _vacationRepository.GetAllVacationsAsync())
                           // as response DTOs
                           .Select(vacation => vacation.toVacationResponse())
                           // keep those that have the end date >= current date
                           .Where(vacation => vacation.EndDate >= currentDate)
                           // and add them into a list
                           .ToList();
                }
                else if (current == false && history == true)
                {
                    DateOnly currentDate = DateOnly.Parse(DateTime.Now.Date.ToString().Split(' ')[0]);
                    // Get all vacations
                    return (await _vacationRepository.GetAllVacationsAsync())
                           // as response DTOs
                           .Select(vacation => vacation.toVacationResponse())
                           // keep those that have the end date < current date
                           .Where(vacation => vacation.EndDate < currentDate)
                           // and add them into a list
                           .ToList();
                }
                else if (current == true && history == true)
                {
                    // Get all vacations
                    return (await _vacationRepository.GetAllVacationsAsync())
                           .Select(vacation => vacation.toVacationResponse())
                           .ToList();
                }
                else
                {
                    return new List<VacationResponse>();
                }
            }

            if (current == true && history == false)
            {
                DateOnly currentDate = DateOnly.Parse(DateTime.Now.Date.ToString().Split(' ')[0]);
                // Get all vacations
                return (await _vacationRepository.GetAllVacationsAsync())
                       // as response DTOs
                       .Select(vacation => vacation.toVacationResponse())
                       // keep those that have the end date >= current date
                       .Where(vacation => vacation.EndDate >= currentDate
                       // and the user id equal to the one given as parameter
                       && vacation.ApplicationUserId == UserId)
                       // Finally, add them into a list
                       .ToList();
            }
            else if (current == false && history == true)
            {
                DateOnly currentDate = DateOnly.Parse(DateTime.Now.Date.ToString().Split(' ')[0]);
                // Get all vacations
                return (await _vacationRepository.GetAllVacationsAsync())
                       // as response DTOs
                       .Select(vacation => vacation.toVacationResponse())
                       // keep those that have the end date < current date
                       .Where(vacation => vacation.EndDate < currentDate
                       // and the user id equal to the one given as parameter
                       && vacation.ApplicationUserId == UserId)
                       // Finally, add them into a list
                       .ToList();
            }
            else if (current == true && history == true)
            {
                // Get all vacations
                return (await _vacationRepository.GetAllVacationsAsync())
                       .Select(vacation => vacation.toVacationResponse())
                       .Where(vacation => vacation.ApplicationUserId == UserId)
                       .ToList();
            }
            else
            {
                return new List<VacationResponse>();
            }
        }

        public async Task<VacationResponse?> GetVacationByIdAsync(Guid? Id)
        {
            // if the given id is null, return null object
            if(Id == null)
                return null;
            
            // get the vacation by id, if none are found we get null
            Vacation? vacation = await _vacationRepository.GetVacationByIdAsync(Id.Value);

            // in case we get null we return null
            if(vacation == null) 
                return null;

            // return the object as response dto
            return vacation.toVacationResponse();
        }

        public async Task<VacationResponse> UpdateVacationAsync(VacationUpdateRequest? vacationUpdateRequest)
        {
            // vacationUpdateRequest is null
            if(vacationUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(vacationUpdateRequest));
            }

            // StartDate is null
            if (vacationUpdateRequest.StartDate.Equals(null))
            {
                throw new ArgumentException(nameof(vacationUpdateRequest.StartDate));
            }

            // EndDate is null
            if (vacationUpdateRequest.EndDate.Equals(null))
            {
                throw new ArgumentException(nameof(vacationUpdateRequest.EndDate));
            }

            // EndDate < StartDate
            if (vacationUpdateRequest.EndDate < vacationUpdateRequest.StartDate)
            {
                throw new ArgumentException("EndDate can't be before StartDate");
            }

            // get vacation from database
            Vacation? vacationFromDb = await _vacationRepository
                .GetVacationByIdAsync(vacationUpdateRequest.Id);

            // doesn't exist
            if (vacationFromDb == null)
            {
                throw new ArgumentException(nameof(vacationUpdateRequest.Id));
            }

            // Check if the requests dates intersect with existing requests
            bool datesIntersect = await CheckIfDatesIntersect(vacationUpdateRequest.StartDate, vacationUpdateRequest.EndDate, vacationFromDb.ApplicationUserId, vacationFromDb.StartDate, vacationFromDb.EndDate);
            if (datesIntersect)
            {
                throw new ArgumentException("There is an existing vacation request that intersects with the current request");
            }

            // Check if the request exceds the remaining available vacation days
            bool excedsAvailableVacationDays = await CheckIfRequestExcedsMaxDaysNumber(vacationUpdateRequest.StartDate, vacationUpdateRequest.EndDate, vacationFromDb.ApplicationUserId, vacationFromDb.StartDate, vacationFromDb.EndDate);
            if (excedsAvailableVacationDays)
            {
                throw new ArgumentException("Not enough vacation days available");
            }

            
            // Update properties
            vacationFromDb.StartDate = vacationUpdateRequest.StartDate;
            vacationFromDb.EndDate = vacationUpdateRequest.EndDate;

            // Update database
            await _vacationRepository.UpdateVacationAsync(vacationFromDb);
            // and then return as VacationResponse object
            return vacationFromDb.toVacationResponse();
        }

        public async Task<bool> DeleteVacationAsync(Guid? Id)
        {
            // if the given id is null, throw exception
            if (Id == null)
                throw new ArgumentNullException(nameof(Id));

            // get the vacation from database
            Vacation? vacationFromDb = await _vacationRepository.GetVacationByIdAsync(Id.Value);

            // if the object we get is null, return false
            if (vacationFromDb == null)
                return false;

            // remove the object
            return await _vacationRepository.DeleteVacationByIdAsync(Id.Value);
        }

        public async Task<int> GetRemainingVacationDaysAsync(Guid? userId, int year)
        {
            // Initialize with the maximum number of vacation days per year
            int remainingVacationDays = StaticDetails.StaticDetails.MaxVacationDaysPerYear;

            // Get all vacations 
            var allVacations = await _vacationRepository.GetAllVacationsAsync();

            var userVacations = allVacations.Where(vacation =>
                        // that have the ApplicationUserId equal to the given one
                        vacation.ApplicationUserId == userId
                        // and starts in the year equal to the one given as parameter
                        && (vacation.StartDate!.Value.Year.Equals(year)
                            // or ends in the year equal to the one given as parameter
                            || vacation.EndDate!.Value.Year.Equals(year))).ToList();


            foreach(var vacation in userVacations)
            {
                // vacation starts before the parameter year
                if(vacation.StartDate!.Value.Year < year)
                {
                    // use the first day of the parameter year as start date
                    DateOnly StartDate = new DateOnly(year, 1, 1);
                    // get the end date from the vacation obj
                    DateOnly EndDate = (DateOnly)vacation.EndDate!;

                    remainingVacationDays = remainingVacationDays - (await GetUsedWorkingDays(StartDate, EndDate));
                }
                // vacation ends after the parameter year
                else if(vacation.StartDate!.Value.Year < year)
                {
                    // get the start date from the vacation obj
                    DateOnly StartDate = (DateOnly)vacation.StartDate!;
                    // use the last day of the parameter year as end date
                    DateOnly EndDate = new DateOnly(year, 12, 31);

                    remainingVacationDays = remainingVacationDays - (await GetUsedWorkingDays(StartDate, EndDate));
                }
                // the entire vacation is in the same year
                else
                {
                    // get the start date from the vacation obj
                    DateOnly StartDate = (DateOnly)vacation.StartDate!;
                    // get the end date from the vacation obj
                    DateOnly EndDate = (DateOnly)vacation.EndDate!;

                    remainingVacationDays = remainingVacationDays - (await GetUsedWorkingDays(StartDate, EndDate));
                }
            }
            return remainingVacationDays;
        }

        /// <summary>
        /// Returns the number of days between two dates
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <returns>The number of days between two dates</returns>
        public async Task<int> GetUsedWorkingDays(DateOnly startDate, DateOnly endDate)
        {
            int returnNumber = 0;
            // get dictonary of national holidays 
            var nationalHolidaysDictionary = await _nationalHolidaysRepository.GetNationalHolidaysDictionaryAsync();

            for (var day = startDate; !day.Equals(endDate.AddDays(1)); day = day.AddDays(1))
            {
                // Weekends are not accounted
                if (day.DayOfWeek != DayOfWeek.Sunday 
                    && day.DayOfWeek != DayOfWeek.Saturday
                    // National holidays are not accounted
                    && !nationalHolidaysDictionary.ContainsKey(day))
                { returnNumber++; }
            }

            return returnNumber;
        }

        /// <summary>
        /// Checks if a vacation request exceds the maximum available vacation days
        /// </summary>
        /// <param name="inputStartDate">Start date of the vacation</param>
        /// <param name="inputEndDate">End date of the vacation</param>
        /// <param name="userId">The id of the user that requests the vacation</param>
        /// <param name="currentVacationStartDate">If the request is an update you have to supply the current vacation object from the database start date</param>
        /// <param name="currentVacationEndDate">If the request is an update you have to supply the current vacation object from the database end date</param>
        /// <returns>True if it exceds the maximum available vacation days; otherwise false</returns>
        public async Task<bool> CheckIfRequestExcedsMaxDaysNumber(DateOnly? inputStartDate, DateOnly? inputEndDate, Guid? userId, DateOnly? currentVacationStartDate = null, DateOnly? currentVacationEndDate = null)
        {
            if (inputStartDate!.Value.Year != inputEndDate!.Value.Year)
            {
                // Start date's year
                int startDateYear = inputStartDate.Value.Year;

                // End date's year
                int endDateYear = inputEndDate.Value.Year;

                // Auxiliary end date for the lower year (last day of the year)
                DateOnly auxEndDate = new DateOnly(startDateYear, 12, 31);

                // Auxiliary start date for the upper year (first day of the year)
                DateOnly auxStartDate = new DateOnly(endDateYear, 1, 1);

                // Remaining available days
                int firstYearRemainingDays = await GetRemainingVacationDaysAsync(userId, startDateYear);
                int secondYearRemainingDays = await GetRemainingVacationDaysAsync(userId, endDateYear);

                // Current object days (relevant for update)
                int firstYearCurrentDays = 0;
                int secondYearCurrentDays = 0;
                // If it is an update request, substract the current vacation days
                if(currentVacationStartDate != null && currentVacationEndDate != null
                    && currentVacationEndDate.Value.Year > currentVacationStartDate.Value.Year) // different year
                {
                    // we need aux dates again
                    DateOnly auxEndDateCurrentVacation = new DateOnly(currentVacationStartDate.Value.Year, 12, 31);
                    DateOnly auxStartDateCurrentVacation = new DateOnly(currentVacationEndDate.Value.Year, 1, 1);

                    firstYearCurrentDays = await GetUsedWorkingDays((DateOnly)currentVacationStartDate, auxEndDateCurrentVacation);
                    secondYearCurrentDays = await GetUsedWorkingDays(auxStartDateCurrentVacation, (DateOnly)currentVacationEndDate);

                }

                // Days needed for the request
                int firstYearVacationDays = await GetUsedWorkingDays((DateOnly)inputStartDate, auxEndDate);
                int secondYearVacationDays = await GetUsedWorkingDays(auxStartDate, (DateOnly)inputEndDate);

                // Check for both years if it exceds the maximum number of vacation days
                // if it is not an update request firstYearCurrentDays and secondYearCurrentDays won't affect the operation
                if (firstYearRemainingDays - firstYearVacationDays - firstYearCurrentDays < 0
                    || secondYearRemainingDays - secondYearVacationDays - secondYearCurrentDays < 0)
                {
                    return true;
                }
            }
            else
            {
                int remainingDays = await GetRemainingVacationDaysAsync(userId, inputStartDate.Value.Year);

                int vacationDays = await GetUsedWorkingDays((DateOnly)inputStartDate, (DateOnly)inputEndDate);

                // For update
                int currentDays = 0;
                if(currentVacationStartDate != null && currentVacationEndDate != null
                    && currentVacationEndDate.Value.Year == currentVacationStartDate.Value.Year) // same year
                {
                    currentDays = await GetUsedWorkingDays((DateOnly)currentVacationStartDate, (DateOnly)currentVacationEndDate);
                }

                if (remainingDays - vacationDays - currentDays < 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///  Check if a start date and an end date intersects with the dates of an existing vacation request of the specified user
        /// </summary>
        /// <param name="inputStartDate">Start date to check</param>
        /// <param name="inputEndDate">End date to check</param>
        /// <param name="userId">The id of the user to get existing vacations from</param>
        /// <param name="currentVacationStartDate">If used for update, you have to pass the current request's start date</param>
        /// <param name="currentVacationEndDate">If used for update, you have to pass the current request's end date</param>
        /// <returns>True if it intersects; otherwise false</returns>
        async Task<bool> CheckIfDatesIntersect(DateOnly? inputStartDate, DateOnly? inputEndDate, Guid? userId, DateOnly? currentVacationStartDate = null, DateOnly? currentVacationEndDate = null)
        {
            if (inputStartDate == null || inputEndDate == null || userId == null)
                throw new ArgumentNullException("inputStartDate, inputEndDate and userId can't be null");

            // Dictionary with start date key, end date value pairs for all user's existing vacation requests
            var vacationsDictionary = await _vacationRepository.GetVacationsDictionaryAsync(userId);
            if (vacationsDictionary.Count == 0)
                return false;

            // If it is used for an update request, remove the current start date key, end date value pair from the dictionary
            if (currentVacationStartDate != null && currentVacationEndDate != null)
            {
                vacationsDictionary.Remove((DateOnly)currentVacationStartDate);
            }

            foreach(KeyValuePair<DateOnly, DateOnly> kvp in vacationsDictionary)
            {
                if (inputEndDate < kvp.Key || inputStartDate > kvp.Value) continue;
                else return true;
            }
            return false;
        }
    }
}
