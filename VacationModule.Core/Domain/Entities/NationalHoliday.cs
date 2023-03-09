using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationModule.Core.Domain.Entities
{
    /// <summary>
    /// The model of national holiday
    /// It's properties are:
    /// Id which is the unique id of the request (primary key). The type is Guid beacuse it allows for unlimited number of requests (eg. if we choose int type we will be limited by the maximum limit of this type)
    /// HolidayName is the name of the respective holiday. The type is nullable string
    /// HolidayDate is the date of the respective holiday. The type is DateOnly because we only care about the actual date (eg. if we choose DateTime we have unnecesary time related objects in memory)
    /// </summary>
    public class NationalHoliday
    {
        public Guid Id { get; set; }
        public string? HolidayName { get; set; }    
        public DateOnly? HolidayDate { get; set; }   
    }
}
