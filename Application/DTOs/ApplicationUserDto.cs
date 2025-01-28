

namespace Application.DTOs
{
    public class ApplicationUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty; 
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; } 
        public List<UserFileDto> UserFiles { get; set; } = new List<UserFileDto>();
    }
}
