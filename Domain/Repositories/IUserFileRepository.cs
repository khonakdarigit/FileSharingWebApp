using Domain.Entities;
using Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserFileRepository 
    {
        Task<UserFile> New(UserFile model);
        Task<IEnumerable<UserFile>> GetUserFileWithDetailsByUserId(string userId);
        Task<UserFile> GetFileWithDetails(Guid id);
        Task Modify(UserFile model);
    }
}
