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
    /// Data Transfer Object class for updating an existing vacation
    /// 
    /// </summary>
    public class VacationUpdateRequest
    {
        public Guid Id { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
      //  public Guid ApplicationUserId { get; set; }

        /// <summary>
        /// This method converts the DTO into the model
        /// </summary>
        /// <returns>Returns the vacation object after converting it</returns>
        public Vacation toVacation()
        {
            return new Vacation()
            {
                Id = Id,
                StartDate = StartDate,
                EndDate = EndDate,
           //     ApplicationUserId = ApplicationUserId
            };
        }
    }
}
