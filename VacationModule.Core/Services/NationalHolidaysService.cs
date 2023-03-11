
using NationalHolidayModule.Core.DTO;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.Domain.RepositoryContracts;
using VacationModule.Core.DTO;
using VacationModule.Core.ServiceContracts;

namespace VacationModule.Core.Services
{
    public class NationalHolidaysService : INationalHolidaysService
    {
        private readonly INationalHolidayRepository _nationalHolidayRepository;

        public NationalHolidaysService(INationalHolidayRepository nationalHolidayRepository)
        {

            _nationalHolidayRepository = nationalHolidayRepository;
        }
        public async Task<NationalHolidayResponse> AddNationalHolidayAsync(NationalHolidayAddRequest? nationalHolidayAddRequest)
        {
            // nationalHolidayAddRequest is null
            if(nationalHolidayAddRequest == null)
            {
                throw new ArgumentNullException(nameof(nationalHolidayAddRequest));
            }

            // HolidayName is null
            if(nationalHolidayAddRequest.HolidayName == null) 
            {
                throw new ArgumentException(nameof(nationalHolidayAddRequest.HolidayName));
            }

            // HolidayDate is null
            if (nationalHolidayAddRequest.HolidayDate.Equals(null))
            {
                throw new ArgumentException(nameof(nationalHolidayAddRequest.HolidayDate));
            }

            // Convert object from NationalHolidayAddRequest to NationalHoliday type
            NationalHoliday nationalHoliday = nationalHolidayAddRequest.toNationalHoliday();

            // Generate Id
            nationalHoliday.Id = Guid.NewGuid();

            // Add Holiday object into _nationalHolidays
            await _nationalHolidayRepository.AddNationalHolidayAsync(nationalHoliday);

            return nationalHoliday.toNationalHolidayResponse();
        }

        public async Task<List<NationalHolidayResponse>> GetAllNationalHolidaysAsync()
        {
            // we have to convert the NationalHoliday objects to NationalHolidayResponse
            return (await _nationalHolidayRepository.GetAllNationalHolidaysAsync())
                .Select(nationalHoliday => nationalHoliday.toNationalHolidayResponse()).ToList();
        }

        public async Task<NationalHolidayResponse?> GetNationalHolidayByIdAsync(Guid? Id)
        {
            // if the given id is null, return null object
            if(Id == null)
                return null;
            
            // get the national holiday by id, if none are found we get null
            NationalHoliday? nationalHoliday = await _nationalHolidayRepository.GetNationalHolidayByIdAsync(Id.Value);

            // in case we get null we return null
            if(nationalHoliday == null) 
                return null;

            // return the object as response dto
            return nationalHoliday.toNationalHolidayResponse();
        }

        public async Task<NationalHolidayResponse> UpdateNationalHolidayAsync(NationalHolidayUpdateRequest? nationalHolidayUpdateRequest)
        {
            // nationalHolidayUpdateRequest is null
            if(nationalHolidayUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(nationalHolidayUpdateRequest));
            }

            // HolidayName is null
            if (nationalHolidayUpdateRequest.HolidayName == null)
            {
                throw new ArgumentException(nameof(nationalHolidayUpdateRequest.HolidayName));
            }
            // HolidayDate is null
            if (nationalHolidayUpdateRequest.HolidayDate == null)
            {
                throw new ArgumentException(nameof(nationalHolidayUpdateRequest.HolidayDate));
            }

            // get national holiday from database
            NationalHoliday? nationalHolidayFromDb = await _nationalHolidayRepository
                .GetNationalHolidayByIdAsync(nationalHolidayUpdateRequest.Id);

            // doesn't exist
            if (nationalHolidayFromDb == null)
            {
                throw new ArgumentException(nameof(nationalHolidayUpdateRequest.Id));
            }
            else
            { // update
                nationalHolidayFromDb.HolidayName = nationalHolidayUpdateRequest.HolidayName;
                nationalHolidayFromDb.HolidayDate = nationalHolidayUpdateRequest.HolidayDate;

                await _nationalHolidayRepository.UpdateNationalHolidayAsync(nationalHolidayFromDb);
            }

            // and then return as NationalHolidayResponse object
            return nationalHolidayFromDb.toNationalHolidayResponse();
        }

        public async Task<bool> DeleteNationalHolidayAsync(Guid? Id)
        {
            // if the given id is null, throw exception
            if (Id == null)
                throw new ArgumentNullException(nameof(Id));

            // get the national holiday from database
            NationalHoliday? nationalHolidayFromDb = await _nationalHolidayRepository.GetNationalHolidayByIdAsync(Id.Value);

            // if the object we get is null, return false
            if (nationalHolidayFromDb == null)
                return false;

            // remove the object
            return await _nationalHolidayRepository.DeleteNationalHolidayByIdAsync(Id.Value);
        }

        public async Task<Dictionary<DateOnly, string?>> GetListToDictionaryAsync()
        {
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
                    dictionaryToReturn[day.HolidayDate.Value] = day.HolidayName;
                }
            }

            return dictionaryToReturn;
        }

        public async Task<List<NationalHolidayResponse>> UpdateYearToAsync(int year)
        {
            if(year < 0)
            {
                throw new ArgumentException(nameof(year));
            }

            // get the list of all national holidays
            var nationalHolidaysFromGet = await _nationalHolidayRepository.GetAllNationalHolidaysAsync();

            // for each holiday, update the year 
            foreach (var day in nationalHolidaysFromGet)
            {
                day.HolidayDate = new DateOnly(year, day.HolidayDate!.Value.Month, day.HolidayDate.Value.Day);
                await _nationalHolidayRepository.UpdateNationalHolidayAsync(day);
            }

            // get the updated list
            var nationalHolidaysFromGetUpdated = (
                // get all national holiday objects
                await _nationalHolidayRepository.GetAllNationalHolidaysAsync())
                // convert them to response DTO
                .Select(temp => temp.toNationalHolidayResponse())
                // to list
                .ToList();

            return nationalHolidaysFromGetUpdated;
        }
    }
}
