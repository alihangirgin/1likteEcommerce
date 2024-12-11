using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1likteEcommerce.Core.Services
{
    public interface IUserService
    {
        string GenerateJwtToken(IdentityUser user);
    }
}
