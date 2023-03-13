using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.Domain.Entities;

namespace VacationModule.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Vacation entity
    /// </summary>
    public interface IVacationRepository
    {
        /// <summary>
        /// Adds a new vacation object to the database
        /// </summary>
        /// <param name="vacation">Vacation object to add</param>
        /// <returns>The vacation object after adding it to the database</returns>
        Task<Vacation> AddVacationAsync(Vacation vacation);

        /// <summary>
        /// Returns all vacations from the database
        /// </summary>
        /// <returns>All vacations from the table</returns>
        Task<List<Vacation>> GetAllVacationsAsync();   

        /// <summary>
        /// Returns a vacation object based on the given id, otherwise returns null
        /// </summary>
        /// <param name="id">Vacation's id (Guid) to search</param>
        /// <returns>Matching vacation or null</returns>
        Task<Vacation?> GetVacationByIdAsync(Guid id);

        /// <summary>
        /// Deletes a vacation object based on the given id
        /// </summary>
        /// <param name="id">Vacation's id (Guid) to search </param>
        /// <returns>True if the object is deleted succesfully, otherwise returns false</returns>
        Task<bool> DeleteVacationByIdAsync(Guid id);

        /// <summary>
        /// Updates a vacation object based on the given vacation id
        /// </summary>
        /// <param name="vacation">Vacation objetct to update</param>
        /// <returns>The updated vacation object</returns>
        Task<Vacation> UpdateVacationAsync(Vacation vacation);

        /// <summary>
        /// Get a dictionary with start date as key and end date as value for all vacation objects of an user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>Dictionary with start date as key and end date as value for all vacation objects of an user</returns>
        Task<Dictionary<DateOnly, DateOnly>> GetVacationsDictionaryAsync(Guid? userId);
    }
}
