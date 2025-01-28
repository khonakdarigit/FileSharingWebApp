
namespace Domain.Entities
{
    public class FileShare
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserFileId { get; set; } 
        public UserFile UserFile { get; set; }

        public string SharedWithUserId { get; set; } 
        public ApplicationUser SharedWithUser { get; set; }

        public static implicit operator FileShare(UserFile v)
        {
            throw new NotImplementedException();
        }
    }
}
