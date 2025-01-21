using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using FileShare = Domain.Entities.FileShare;

namespace Infrastructure.Repositories
{
    internal class FileShareRepository : Repository<FileShare>, IFileShareRepository
    {
        public FileShareRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FileShare>> AllFileShareWithMe(string userId)
        {
            return await GetAllAsync(
                filter: c => (c.SharedWithUserId == userId || c.UserFile.IsPublic) && c.UserFile.UploadedById != userId,
                include: c => c.Include(c => c.UserFile).ThenInclude(c => c.UploadedBy)
                );
        }
    }
}
