using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IUserFileService
    {
        Task AddAsync(UserFileDto userFile);
        Task<IEnumerable<AccessFileDto>> AllFileShareWithMe(string userId);
        Task<IEnumerable<UserFileDto>> GetUserFileWithDetailsByUserId(string userId);
    }
}
