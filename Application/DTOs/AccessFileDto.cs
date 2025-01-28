

namespace Application.DTOs
{
    public class AccessFileDto
    {
        public Guid UserFileId { get;  set; }
        public string UserId { get;  set; }
        public string? UserName { get;  set; }
        public string FileName { get;  set; }
    }
}
