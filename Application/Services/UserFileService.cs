using Application.DTOs;
using Application.Interface;
using Domain.Entities;
using Domain.Repositories;
using Mapster;


namespace Application.Services
{
    public class UserFileService : IUserFileService
    {
        private readonly IUserFileRepository _userFilesRep;
        private readonly IFileShareRepository _fileShareRep;
        public UserFileService(
            IUserFileRepository userFilesRep,
            IFileShareRepository fileShareRep
            )
        {
            _userFilesRep = userFilesRep;
            _fileShareRep = fileShareRep;
        }

        public Task AddAsync(UserFileDto userFile)
        {
            var model= userFile.Adapt<UserFile>();
            _userFilesRep.AddAsync(model);
        }

        public async Task<IEnumerable<AccessFileDto>> AllFileShareWithMe(string userId)
        {
            var shareFiles = await _fileShareRep.AllFileShareWithMe(userId);
            var toAccessFile = shareFiles.Select(c => new AccessFileDto()
            {
                UserFileId = c.UserFileId,
                UserId = c.UserFile.UploadedById,
                UserName = c.UserFile.UploadedBy.UserName,
                FileName = c.UserFile.Name,
            });
            return toAccessFile;
        }

        public async Task<IEnumerable<UserFileDto>> GetUserFileWithDetailsByUserId(string userId)
        {
            var userFiles = await _userFilesRep.GetUserFileWithDetailsByUserId(userId);
            return userFiles.Adapt<IEnumerable<UserFileDto>>();
        }
    }
}
