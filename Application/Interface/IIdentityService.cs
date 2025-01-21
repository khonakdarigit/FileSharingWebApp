using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IIdentityService
    {
        Task<bool> IsInRoleAsync(string userId, string role);

        ApplicationUser GetUser(string userId);
        Task<ApplicationUser> AddOrModifyUserData(ApplicationUser applicationUser);
    }
}
