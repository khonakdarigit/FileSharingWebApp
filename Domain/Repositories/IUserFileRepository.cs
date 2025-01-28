using Domain.Entities;
using Domain.Repositories.Common;

namespace Domain.Repositories
{
    public interface IUserFileRepository : IRepository<UserFile>
    {
        Task<IEnumerable<UserFile>> GetUserFileWithDetailsByUserId(string userId);
        Task<UserFile?> GetFileWithDetails(Guid id);
        Task<IEnumerable<UserFile>> GetPublicFiles();
    }
}
