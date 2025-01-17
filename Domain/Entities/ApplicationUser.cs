using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePictureUrl { get; set; } 
        public ICollection<UserFile> UserFiles { get; set; } = new List<UserFile>();
    }
}
