using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserFileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long Size { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string UploadedById { get; set; }
        public bool IsPublic { get; set; }
        public List<FileShareDto> SharedWithUsers { get; set; } = new List<FileShareDto>();
    }
}
