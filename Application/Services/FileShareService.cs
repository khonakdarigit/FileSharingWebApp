using Application.DTOs;
using Application.Interface;
using Domain.Repositories;
using Mapster;


namespace Application.Services
{
    public class FileShareService : IFileShareService
    {
        private readonly IFileShareRepository _fileShareRep;
        public FileShareService(
            IFileShareRepository fileShareRep
            )
        {
            _fileShareRep = fileShareRep;
        }

        public async Task<FileShareDto> AddFileShare(FileShareDto fileShareDto)
        {
            var model = fileShareDto.Adapt<Domain.Entities.FileShare>();
            await _fileShareRep.AddAsync(model);
            await _fileShareRep.SaveChangesAsync();
            fileShareDto.Id = model.Id;
            return fileShareDto;
        }

        public async Task DeleteFileShare(FileShareDto fileShare)
        {
            var model = await _fileShareRep.GetByIdAsync(fileShare.Id);
            _fileShareRep.Remove(model);
            await _fileShareRep.SaveChangesAsync();
        }
    }
}
