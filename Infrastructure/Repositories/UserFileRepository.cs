using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserFileRepository : Repository<UserFile>, IUserFileRepository
    {
        public UserFileRepository(
            ApplicationDbContext context) : base(context)
        {

        }

        public async Task<UserFile?> GetFileWithDetails(Guid id)
        {
            var userFile = await FindAsync(
                          filter: c => c.Id == id,
                          include: c => c.Include(c => c.SharedWithUsers));
            return userFile;
        }

        public async Task<IEnumerable<UserFile>> GetUserFileWithDetailsByUserId(string userId)
        {
            var list = await GetAllAsync(
                filter: c => c.UploadedById == userId,
                include: c => c.Include(c => c.SharedWithUsers).ThenInclude(c=>c.SharedWithUser));

            return list;
        }

        public async Task<IEnumerable<UserFile>> GetPublicFiles()
        {
            var list = await GetAllAsync(
                c => c.IsPublic == true,
                c=>c.Include(c=>c.UploadedBy));
            return list;
        }
    }
}
