using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FolderNameDto
    {
        [Required(ErrorMessage = "Folder name cannot be empty.")]
        [StringLength(255, ErrorMessage = "Folder name cannot be longer than 255 characters.")]
        [RegularExpression(@"^[^<>:""/\\|?*]+$", ErrorMessage = "Folder name contains invalid characters.")]
        public string FolderName { get; set; }
    }
}
