using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationModule.Core.Domain.IdentityEntities
{
    /// <summary>
    /// Represents the Identity Users specific to this API
    /// It inherits Identity user, and sets the Id type to be Guid (unlimited values)
    /// </summary>
    public class ApplicationUser: IdentityUser<Guid>
    {

    }
}
