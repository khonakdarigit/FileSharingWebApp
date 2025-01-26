using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IFileShareService
    {
        Task Delete(FileShareDto item);
        Task<FileShareDto> NewFileShare(FileShareDto fileShareDto);
    }
}
