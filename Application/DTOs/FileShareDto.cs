

namespace Application.DTOs
{
    public class FileShareDto
    {
        public Guid Id { get; set; }
        public Guid UserFileId { get; set; }
        public string SharedWithUserId { get; set; }
        public ApplicationUserDto SharedWithUser { get; set; }

    }
}
