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
                filter: c => c.SharedWithUserId == userId && c.UserFile.UploadedById != userId,
                include: c => c.Include(c => c.UserFile).ThenInclude(c => c.UploadedBy)
                );
        }

        public async Task Delete(FileShare model)
        {
            Remove(model);
            await SaveChangesAsync();
        }

        public async Task<FileShare> GetById(Guid id)
        {
            var fileShare = await GetByIdAsync(id);
            return fileShare;
        }

        public async Task NewFileShare(FileShare fileShare)
        {
            await AddAsync(fileShare);
            await SaveChangesAsync();
        }
    }
}
