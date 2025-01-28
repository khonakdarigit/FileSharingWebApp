using Domain.Repositories.Common;
using FileShare = Domain.Entities.FileShare;

namespace Domain.Repositories
{
    public interface IFileShareRepository : IRepository<FileShare>
    {
        Task<IEnumerable<FileShare>> AllFileShareWithMe(string userId);
    }
}
