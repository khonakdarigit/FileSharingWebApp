using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserFile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty; 
        public string FilePath { get; set; } = string.Empty; 
        public long Size { get; set; } 
        public string ContentType { get; set; } = string.Empty; 

        public string UploadedById { get; set; } 
        public ApplicationUser UploadedBy { get; set; } 

        public bool IsPublic { get; set; } = false; 

        public ICollection<FileShare> SharedWithUsers { get; set; } = new List<FileShare>(); 
    }
}
