using VacationModule.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.DTO;

namespace VacationModule.Core.ServiceContracts
{
    /// <summary>
    /// Represents the business logic for manipulating Vacation entity
    /// </summary>
    public interface IVacationsService
    {
        /// <summary>
        /// Adds a vacation object to the list of vacations
        /// </summary>
        /// <param name="vacationAddRequest">Vacation object to add</param>
        /// <param name="UserId">The authenticated user's Id</param>
        /// <returns>The vacation object after adding it (including newly generated id)</returns>
        Task<VacationResponse> AddVacationAsync(VacationAddRequest? vacationAddRequest, Guid? UserId);

        /// <summary>
        /// Returns all vacations
        /// </summary>
        /// <param name="current">If true returns vacations that will start in the future, or that are not finished yet</param>
        /// <param name="history">If true returns vacations that have ended</param>
        /// <param name="UserId">If not null, returns vacations of the user that has this id</param>
        /// <returns></returns>
        Task<List<VacationResponse>> GetAllVacationsAsync(bool current = true, bool history = false, Guid? UserId = null);

        /// <summary>
        /// Returns the vacation based on the given id
        /// </summary>
        /// <param name="Id">The id of the vacation to get</param>
        /// <returns>Matching vacation response object</returns>
        Task<VacationResponse?> GetVacationByIdAsync(Guid? Id);

        /// <summary>
        /// Updates the specified vacation details based on the given vacation id
        /// </summary>
        /// <param name="vacationUpdateRequest">Vacation details to update, including the id</param>
        /// <returns>The vacation response object after updating it</returns>
        Task<VacationResponse> UpdateVacationAsync(VacationUpdateRequest? vacationUpdateRequest);

        /// <summary>
        /// Delete a vacation based on the given id
        /// </summary>
        /// <param name="Id">the id of the vacation to delete</param>
        /// <returns>True if the object is deleted succesfully, otherwise False</returns>
        Task<bool> DeleteVacationAsync(Guid? Id);

        /// <summary>
        /// For an user, get the remaining number of vacation days available for a specified year
        /// </summary>
        /// <param name="userId"> The id of the user </param>
        /// <param name="year"> The year to get the number of remaining days for </param>
        /// <returns>The remaining number of vacation days available for a specified year</returns>
        Task<int> GetRemainingVacationDaysAsync(Guid? userId, int year);

    }  
}
