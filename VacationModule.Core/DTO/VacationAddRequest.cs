using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.Domain.Entities;
using VacationModule.Core.Domain.IdentityEntities;

namespace VacationModule.Core.DTO
{
    /// <summary>
    /// Data Transfer Object class for adding a new vacation
    /// The id of the Vacation is not known at the time of requesting a vacation
    /// 
    /// </summary>
    public class VacationAddRequest
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// This method converts the DTO into the model
        /// </summary>
        /// <returns>Returns the vacation object after converting it</returns>
        public Vacation toVacation()
        {
            return new Vacation()
            {
                StartDate = StartDate,
                EndDate = EndDate
            };
        }
    }
}
