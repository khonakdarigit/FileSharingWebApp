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

        public async Task AddUserFileAsync(UserFileDto userFile)
        {
            var model = userFile.Adapt<UserFile>();
            await _userFilesRep.AddAsync(model);
            await _userFilesRep.SaveChangesAsync();
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
            }).ToList();

            var pubAccessFile = publicFiles.Where(c => c.UploadedById != userId).Select(c => new AccessFileDto()
            {
                UserFileId = c.Id,
                UserId = c.UploadedById,
                UserName = c.UploadedBy.UserName,
                FileName = c.Name
            });

            toAccessFile.AddRange(pubAccessFile);

            return toAccessFile.OrderBy(c => c.UserFileId);
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

        public async Task UpdateUserFileAsync(UserFileDto file)
        {
            UserFile userFile = await _userFilesRep.GetFileWithDetails(file.Id);
            userFile.IsPublic = file.IsPublic;
            _userFilesRep.Update(userFile);
            await _userFilesRep.SaveChangesAsync();
        }

        public async Task DeleteUserFileAsync(UserFileDto file)
        {
            UserFile userFile = await _userFilesRep.GetFileWithDetails(file.Id);
            _userFilesRep.Remove(userFile);
            await _userFilesRep.SaveChangesAsync();
        }
    }
}
