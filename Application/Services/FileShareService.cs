using Application.DTOs;
using Application.Interface;
using Domain.Entities;
using Domain.Repositories;
using Domain.Repositories.Common;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileShare = Domain.Entities.FileShare;

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

        public async Task<FileShareDto> NewFileShare(FileShareDto fileShareDto)
        {
            var model = fileShareDto.Adapt<Domain.Entities.FileShare>();
            await _fileShareRep.NewFileShare(model);
            fileShareDto.Id = model.Id;
            return fileShareDto;
        }

        public async Task Delete(FileShareDto fileShare)
        {
            var model = await _fileShareRep.GetById(fileShare.Id);
            await _fileShareRep.Delete(model);
        }
    }
}
