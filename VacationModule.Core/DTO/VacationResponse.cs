using VacationModule.Core.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.Domain.IdentityEntities;

namespace VacationModule.Core.DTO
{
    /// <summary>
    /// Data Transfer Object class that is used as return type for most of VacationsService methods
    /// </summary>
    public class VacationResponse
    {
        public Guid Id { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public Guid ApplicationUserId { get; set; }

        // The Equals method has to be overriden because it only checks the reference of the object,
        // not the actual value
        // Now it compares the current object to another object of VacationResponse type and it
        // returns true if both values (not references) are the same
        // Otherwise it returns false
        public override bool Equals(object? obj)
        {
            // if the object to compare is null return false
            if (obj == null) return false;

            // if the object to compare is not VacationResponse type return false
            if (obj.GetType() != typeof(VacationResponse)) return false;

            // convert the reference object to an instance of the VacationResponse class
            // to access it's properties
            VacationResponse vacation_to_compare = (VacationResponse)obj;

            // compare the values
            return this.Id == vacation_to_compare.Id
                && this.StartDate == vacation_to_compare.StartDate
                && this.EndDate == vacation_to_compare.EndDate
                && this.ApplicationUserId == vacation_to_compare.ApplicationUserId;
        }

        /// <summary>
        /// Converts a VacationResponse to a VacationUpdateRequest
        /// </summary>
        /// <returns>VacationUpdateRequest object</returns>
        public VacationUpdateRequest toVacationUpdateRequest()
        {
            return new VacationUpdateRequest()
            {
                Id = this.Id,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
               // ApplicationUserId = this.ApplicationUserId
            };
        }
    }

    /// <summary>
    /// This is an extension class for the Vacation model
    /// It adds a new satatic method to the class without actually changing it, respecting Open-Closed principle
    /// I added it because, to my knowledge, in a real working enviroment we are not supposed to change existing code if we are not trying to fix a bug
    /// </summary>
    public static class VacationExtensions
    {
        /// <summary>
        /// This method converts a Vacation object to a VacationResponse DTO
        /// </summary>
        /// <param name="vacation">this is the instantiated object that calls the method</param>
        /// <returns> Returns the national holiday response object after converting it</returns>
        public static VacationResponse toVacationResponse(this Vacation vacation)
        {
            return new VacationResponse()
            {
                Id = vacation.Id,
                StartDate = vacation.StartDate,
                EndDate = vacation.EndDate,
                ApplicationUserId = vacation.ApplicationUserId
            };
        }
    }
}
