using Domain.Entities;


namespace Application.Interface
{
    public interface IIdentityService
    {
        Task<bool> IsInRoleAsync(string userId, string role);
        ApplicationUser GetUser(string userId);
        Task<ApplicationUser> AddOrModifyUserData(ApplicationUser applicationUser);
    }
}
