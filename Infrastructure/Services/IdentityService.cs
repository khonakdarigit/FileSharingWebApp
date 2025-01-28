using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Services
{

    public class IdentityService: IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityService(
             UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }

        public async Task<ApplicationUser> AddOrModifyUserData(ApplicationUser applicationUser)
        {
            var user = await _userManager.FindByIdAsync(applicationUser.Id);

            if (user != null)
            {
                user.UserName = applicationUser.UserName;
                user.Email = applicationUser.Email;
                user.ProfilePictureUrl = applicationUser.ProfilePictureUrl; 

                await _userManager.UpdateAsync(user);
            }
            else
            {
                await _userManager.CreateAsync(applicationUser);
            }

            return applicationUser;
        }


        public ApplicationUser GetUser(string userId)
        {
            return _userManager.FindByIdAsync(userId).Result;
        }


        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _userManager.IsInRoleAsync(user, role);
            }
            return false;
        }

    }
}
