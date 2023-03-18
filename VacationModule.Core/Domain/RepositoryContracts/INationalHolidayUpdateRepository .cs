using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.Domain.Entities;

namespace VacationModule.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents the updated data access logic for managing NationalHoliday entity. 
    /// </summary>
    public interface INationalHolidayUpdateRepository : INationalHolidayRepository
    {
        /// <summary>
        /// Adds a new national holiday object to the database
        /// </summary>
        /// <param name="nationalHoliday">NationalHoliday object to add</param>
        /// <returns>The national holiday object after adding it to the database</returns>
        Task<NationalHoliday> AddNationalHolidayAsync(NationalHoliday nationalHoliday);

        /// <summary>
        /// Returns all national holidays from the database
        /// </summary>
        /// <returns>All national holidays from the table</returns>
        Task<List<NationalHoliday>> GetAllNationalHolidaysAsync();   

        /// <summary>
        /// Returns a national holiday object based on the given id, otherwise returns null
        /// </summary>
        /// <param name="id">National holiday's id (Guid) to search</param>
        /// <returns>Matching national holiday or null</returns>
        Task<NationalHoliday?> GetNationalHolidayByIdAsync(Guid id);

        /// <summary>
        /// Deletes a national holiday object based on the given id
        /// </summary>
        /// <param name="id">National holiday's id (Guid) to search </param>
        /// <returns>True if the object is deleted succesfully, otherwise returns false</returns>
        Task<bool> DeleteNationalHolidayByIdAsync(Guid id);

        /// <summary>
        /// Updates a national holiday object based on the given national holiday id
        /// </summary>
        /// <param name="nationalHoliday">National holiday objetct to update</param>
        /// <returns>The updated national holiday object</returns>
        Task<NationalHoliday> UpdateNationalHolidayAsync(NationalHoliday nationalHoliday);

        /// <summary>
        /// Returns a dictionary with HolidayDate-HolidayName key-value pair of all national holiday objects
        /// </summary>
        /// <returns>Dictionary with HolidayDate-HolidayName key-value pair of all national holiday objects</returns>
        Task<Dictionary<DateOnly, string?>> GetNationalHolidaysDictionaryAsync();

        /// <summary>
        /// Returns a dictionary with HolidayDate-HolidayName key-value pair of all national holiday objects changing the year to the given one/
        /// </summary>
        /// <param name="year">The year of all national holidays in the dictionary</param>
        /// <returns></returns>
        Task<Dictionary<DateOnly, string?>> GetNationalHolidaysDictionaryYearAsync(int? year);
    }
}
