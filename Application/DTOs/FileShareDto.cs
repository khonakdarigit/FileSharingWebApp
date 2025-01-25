using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
