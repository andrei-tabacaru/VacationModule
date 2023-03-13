using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationModule.Core.Domain.IdentityEntities;

namespace VacationModule.Core.Domain.Entities
{
    /// <summary>
    /// The model of Vacation
    /// It's properties are:
    /// Id which is the unique id of the request (primary key). The type is Guid beacuse it allows for unlimited number of requests (eg. if we choose int type we will be limited by the maximum limit of this type)
    /// StartDate which is the starting date of the vacation request. The type is DateOnly because we only care about the actual date (eg. if we choose DateTime we have unnecesary time related objects in memory)
    /// EndDate which is the ending date of the vacation request. The type is the same of the DataStart
    /// ApplicationUserId is the id of the user that creates the vacation. It is a foreign key
    /// ApplicationUser is the actual user object that refers to the ApplicatonUserId foreign key
    /// VacationWorkingDays stores the number of working days that an instance uses
    /// </summary>
    public class Vacation
    {
        public Guid Id { get; set; }    
        public DateOnly? StartDate { get; set; } 
        public DateOnly? EndDate { get; set;}
        // A vacation is created by an user
        public Guid ApplicationUserId { get; set; } 
    }
}
