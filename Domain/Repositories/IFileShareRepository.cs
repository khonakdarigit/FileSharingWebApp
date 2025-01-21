using FileShare = Domain.Entities.FileShare;

namespace Domain.Repositories
{
    public interface IFileShareRepository
    {
        Task<IEnumerable<FileShare>> AllFileShareWithMe(string userId);
    }
}
