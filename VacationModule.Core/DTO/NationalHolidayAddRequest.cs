using DateOnlyTimeOnly.AspNet.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VacationModule.Core.Domain.Entities;

namespace VacationModule.Core.DTO
{
    /// <summary>
    /// Data Transfer Object class for adding a new national holiday
    /// The id of the NationalHoliday is not known at the time of requesting a holiday
    /// 
    /// </summary>
    public class NationalHolidayAddRequest
    {
        public string? HolidayName { get; set; }
        public DateOnly? HolidayDate { get; set; }

        /// <summary>
        /// This method converts the DTO into the model
        /// </summary>
        /// <returns>Returns the national holiday object after converting it</returns>
        public NationalHoliday toNationalHoliday()
        {
            return new NationalHoliday()
            {
                HolidayName = HolidayName,
                HolidayDate = HolidayDate,
            };
        }
    }
}
