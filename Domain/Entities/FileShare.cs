﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FileShare
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserFileId { get; set; } 
        public UserFile UserFile { get; set; }

        public string SharedWithUserId { get; set; } 
        public ApplicationUser SharedWithUser { get; set; } 
    }
}
