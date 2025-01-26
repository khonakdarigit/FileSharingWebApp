using Application.DTOs;
using Application.Interface;
using Domain.Entities;
using Domain.Repositories;
using Mapster;
using FileShare = Domain.Entities.FileShare;

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

        public async Task NewAsync(UserFileDto userFile)
        {
            var model = userFile.Adapt<UserFile>();
            await _userFilesRep.New(model);
        }

        public async Task<IEnumerable<AccessFileDto>> AllFileShareWithMe(string userId)
        {
            var shareFiles = await _fileShareRep.AllFileShareWithMe(userId);

            var publicFiles = await _userFilesRep.GetPublicFiles();

            var toAccessFile = shareFiles.Select(c => new AccessFileDto()
            {
                UserFileId = c.UserFileId,
                UserId = c.UserFile.UploadedById,
                UserName = c.UserFile.UploadedBy.UserName,
                FileName = c.UserFile.Name,
            });


            return toAccessFile;
        }

        public async Task<UserFileDto> GetFileWithDetails(Guid id)
        {
            UserFile userFile = await _userFilesRep.GetFileWithDetails(id);
            return userFile.Adapt<UserFileDto>();
        }

        public async Task<IEnumerable<UserFileDto>> GetUserFileWithDetailsByUserId(string userId)
        {
            var userFiles = await _userFilesRep.GetUserFileWithDetailsByUserId(userId);
            return userFiles.Adapt<IEnumerable<UserFileDto>>();
        }

        public async Task ModifyAsync(UserFileDto file)
        {
            UserFile userFile = await _userFilesRep.GetFileWithDetails(file.Id);
            userFile.IsPublic = file.IsPublic;
            await _userFilesRep.Modify(userFile);
        }

        public async Task Delete(UserFileDto file)
        {
            UserFile userFile = await _userFilesRep.GetFileWithDetails(file.Id);
            await _userFilesRep.Delete(userFile);
        }
    }
}
