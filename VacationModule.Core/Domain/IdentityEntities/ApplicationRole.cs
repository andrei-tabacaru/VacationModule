using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationModule.Core.Domain.IdentityEntities
{
    /// <summary>
    /// Represents the Identity roles specific to this API
    /// </summary>
    public class ApplicationRole: IdentityRole<Guid>
    {
    }
}
