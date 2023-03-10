
using NationalHolidayModule.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.DTO;

namespace VacationModule.Core.ServiceContracts
{
    /// <summary>
    /// Represents the business logic for manipulating NationalHoliday entity
    /// </summary>
    public interface INationalHolidaysService
    {
        /// <summary>
        /// Adds a national holiday object to the list of national holidays
        /// </summary>
        /// <param name="nationalHolidayAddRequest">NationalHoliday object to add</param>
        /// <returns>The national holiday object after adding it (including newly generated id)</returns>
        NationalHolidayResponse AddNationalHoliday(NationalHolidayAddRequest? nationalHolidayAddRequest);

        /// <summary>
        /// Returns all national holidays
        /// </summary>
        /// <returns>All national holidays from the list as List of NationalHolidayResponse</returns>
        List<NationalHolidayResponse> GetAllNationalHolidays();

        /// <summary>
        /// Returns the national holiday based on the given id
        /// </summary>
        /// <param name="Id">The id of the national holiday to get</param>
        /// <returns>Matching national holiday response object</returns>
        NationalHolidayResponse? GetNationalHolidayById(Guid? Id);

        /// <summary>
        /// Updates the specified national holiday details based on the given national holiday id
        /// </summary>
        /// <param name="nationalHolidayUpdateRequest">NationalHoliday details to update, including the id</param>
        /// <returns>The national holiday response object after updating it</returns>
        NationalHolidayResponse UpdateNationalHoliday(NationalHolidayUpdateRequest? nationalHolidayUpdateRequest);

        /// <summary>
        /// Delete a national holiday based on the given id
        /// </summary>
        /// <param name="Id">the id of the national holiday to delete</param>
        /// <returns>True if the object is deleted succesfully, otherwise False</returns>
        bool DeleteNationalHoliday(Guid? Id);

        /// <summary>
        /// Get a dictionary with HolidayDate-HolidayName key-value pair for all national holiday objects
        /// </summary>
        /// <returns>Dictionary with HolidayDate-HolidayName key-value pair for all national holiday objects</returns>
        Dictionary<DateOnly, string?> GetListToDictionary();

        /// <summary>
        /// Updates all existing national holiday's year to the given year
        /// </summary>
        /// <param name="year">The year to upate all existing national holidays year to</param>
        /// <returns>List of updated national holiday response objects</returns>
        List<NationalHolidayResponse> UpdateYearTo(int year);
    }
}
