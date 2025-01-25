using Application.DTOs;
using Application.Interface;
using Domain.Repositories;
using Domain.Repositories.Common;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
