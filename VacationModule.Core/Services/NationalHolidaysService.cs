using NationalHolidayModule.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.DTO;
using VacationModule.Core.ServiceContracts;

namespace VacationModule.Core.Services
{
    public class NationalHolidaysService : INationalHolidaysService
    {
        private readonly List<NationalHoliday> _nationalHolidays;

        public NationalHolidaysService()
        {
            _nationalHolidays= new List<NationalHoliday>();
        }
        public NationalHolidayResponse AddNationalHoliday(NationalHolidayAddRequest? nationalHolidayAddRequest)
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
            _nationalHolidays.Add(nationalHoliday);

            return nationalHoliday.toNationalHolidayResponse();
        }

        public List<NationalHolidayResponse> GetAllNationalHolidays()
        {
            // we have to convert the NationalHoliday objects to NationalHolidayResponse
            return _nationalHolidays.Select(nationalHoliday => nationalHoliday.toNationalHolidayResponse()).ToList();
        }

        public NationalHolidayResponse? GetNationalHolidayById(Guid? Id)
        {
            // if the given id is null, return null object
            if(Id == null)
                return null;
            
            // get the national holiday by id, if none are found we get null
            NationalHoliday? nationalHoliday = _nationalHolidays.FirstOrDefault(temp => temp.Id == Id);

            // in case we get null we return null
            if(nationalHoliday == null) 
                return null;

            // return the object as response dto
            return nationalHoliday.toNationalHolidayResponse();
        }

        public NationalHolidayResponse UpdateNationalHoliday(NationalHolidayUpdateRequest? nationalHolidayUpdateRequest)
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

            // duplicate data members
            if (_nationalHolidays.Where(temp => temp.HolidayName == nationalHolidayUpdateRequest?.HolidayName).Count() > 0
                || _nationalHolidays.Where(temp => temp.HolidayDate.Equals(nationalHolidayUpdateRequest.HolidayDate)).Count() > 0)
            {
                throw new ArgumentException("HolidayName or HolidayDate already exists");
            }

            // doesn't exist
            NationalHoliday? nationalHolidayFromList = _nationalHolidays.FirstOrDefault(temp => temp.Id == nationalHolidayUpdateRequest?.Id);
            if (nationalHolidayFromList == null)
            {
                throw new ArgumentException(nameof(nationalHolidayUpdateRequest.Id));
            }
            else
            { // update
                nationalHolidayFromList.HolidayName = nationalHolidayUpdateRequest.HolidayName;
                nationalHolidayFromList.HolidayDate = nationalHolidayUpdateRequest.HolidayDate;
            }

            // and then return as NationalHolidayResponse object
            return nationalHolidayFromList.toNationalHolidayResponse();
        }

        public bool DeleteNationalHoliday(Guid? Id)
        {
            // if the given id is null, throw exception
            if (Id == null)
                throw new ArgumentNullException(nameof(Id));

            NationalHoliday? nationalHoliday_from_list = _nationalHolidays.FirstOrDefault(temp => temp.Id == Id);

            // if the object we get is null, return false
            if (nationalHoliday_from_list == null)
                return false;

            // remove the object
            _nationalHolidays.RemoveAll(temp => temp.Id == Id);

            return true;
        }

        public Dictionary<DateOnly, string?> GetListToDictionary()
        {
            // get the list of national holidays
            var nationalHolidays_from_get = _nationalHolidays.Select(temp => temp).ToList();

            // new dictionary with DateOnly as key and string as value
            Dictionary<DateOnly, string?> dictionaryToReturn = new Dictionary<DateOnly, string?>();

            // for each holiday, if the holiday date is not null, add it to the dictionary
            foreach (var day in nationalHolidays_from_get)
            {
                if (!day.HolidayDate.Equals(null))
                {
                    dictionaryToReturn[day.HolidayDate.Value] = day.HolidayName;
                }
            }

            return dictionaryToReturn;
        }
    }
}
