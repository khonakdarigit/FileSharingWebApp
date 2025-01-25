using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserFileRepository : Repository<UserFile>, IUserFileRepository
    {
        private readonly IFileShareRepository _fileShareRep;

        public UserFileRepository(
            ApplicationDbContext context,
            IFileShareRepository fileShareRep) : base(context)
        {
            _fileShareRep = fileShareRep;
        }

        public async Task<UserFile> GetFileWithDetails(Guid id)
        {
            var query = await FindAsync(
                          filter: c => c.Id == id,
                          include: c => c.Include(c => c.SharedWithUsers));
            return query;
        }

        public async Task<IEnumerable<UserFile>> GetUserFileWithDetailsByUserId(string userId)
        {
            var query = await GetAllAsync(
                filter: c => c.UploadedById == userId,
                include: c => c.Include(c => c.SharedWithUsers));

            return query;
        }

        public async Task Modify(UserFile model)
        {
            Update(model);
            await SaveChangesAsync();
        }

        public async Task<UserFile> New(UserFile model)
        {
            await AddAsync(model);
            await SaveChangesAsync();
            return model;
        }
    }
}
