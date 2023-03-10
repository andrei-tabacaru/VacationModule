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
    /// Data Transfer Object class for updating an existing national holiday
    /// 
    /// </summary>
    public class NationalHolidayUpdateRequest
    {
        public Guid Id { get; set; }    
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
                Id = Id,
                HolidayName = HolidayName,
                HolidayDate = HolidayDate,
            };
        }
    }
}
