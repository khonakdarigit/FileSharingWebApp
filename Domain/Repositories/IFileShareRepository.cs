using FileShare = Domain.Entities.FileShare;

namespace Domain.Repositories
{
    public interface IFileShareRepository
    {
        Task NewFileShare(FileShare fileShare);
        Task<IEnumerable<FileShare>> AllFileShareWithMe(string userId);
        Task<FileShare> GetById(Guid id);
        Task Delete(FileShare model);
    }
}
