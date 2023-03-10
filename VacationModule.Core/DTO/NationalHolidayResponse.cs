using DateOnlyTimeOnly.AspNet.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.DTO;

namespace NationalHolidayModule.Core.DTO
{
    /// <summary>
    /// Data Transfer Object class that is used as return type for most of NationalHolidaysService methods
    /// </summary>
    public class NationalHolidayResponse
    {
        public Guid Id { get; set; }
        public string? HolidayName { get; set; }
        public DateOnly? HolidayDate { get; set; }

        // The Equals method has to be overriden because it only checks the reference of the object,
        // not the actual value
        // Now it compares the current object to another object of NationalHolidayResponse type and it
        // returns true if both values (not references) are the same
        // Otherwise it returns false
        public override bool Equals(object? obj)
        {
            // if the object to compare is null return false
            if (obj == null) return false;  

            // if the object to compare is not NationalHolidayResponse type return false
            if (obj.GetType() != typeof(NationalHolidayResponse)) return false;

            // convert the reference object to an instance of the NationalHolidayResponse class
            // to access it's properties
            NationalHolidayResponse nationalHoliday_to_compare = (NationalHolidayResponse)obj;

            // compare the values
            return this.Id == nationalHoliday_to_compare.Id
                && this.HolidayName == nationalHoliday_to_compare.HolidayName
                && this.HolidayDate == nationalHoliday_to_compare.HolidayDate; 
        }

        /// <summary>
        /// Converts a NationalHolidayResponse to a NationalHolidayUpdateRequest
        /// </summary>
        /// <returns>NationalHolidayUpdateRequest object</returns>
        public NationalHolidayUpdateRequest toNationalHolidayUpdateRequest()
        {
            return new NationalHolidayUpdateRequest()
            {
                Id = this.Id,
                HolidayName = this.HolidayName,
                HolidayDate = this.HolidayDate,
            };
        }
    }

    /// <summary>
    /// This is an extension class for the NationalHoliday model
    /// It adds a new satatic method to the class without actually changing it, respecting Open-Closed principle
    /// I added it because, to my knowledge, in a real working enviroment we are not supposed to change existing code if we are not trying to fix a bug
    /// </summary>
    public static class NationalHolidayExtensions
    {
        /// <summary>
        /// This method converts a NationalHoliday object to a NationalHolidayResponse DTO
        /// </summary>
        /// <param name="nationalHoliday">this is the instantiated object that calls the method</param>
        /// <returns> Returns the national holiday response object after converting it</returns>
        public static NationalHolidayResponse toNationalHolidayResponse(this NationalHoliday nationalHoliday)
        {
            return new NationalHolidayResponse()
            {
                Id = nationalHoliday.Id,
                HolidayName = nationalHoliday.HolidayName,
                HolidayDate = nationalHoliday.HolidayDate
            };
        }
    }
}
