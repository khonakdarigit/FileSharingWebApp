using Application.DTOs;
using Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileShare = Domain.Entities.FileShare;

namespace Application.DTOs.Config
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserFile, UserFileDto>()
                  .Map(dest => dest.SharedWithUsers, src => src.SharedWithUsers.Adapt<List<FileShareDto>>());

            config.NewConfig<FileShare, FileShareDto>()
                .Map(c => c.SharedWithUser, c => c.SharedWithUser.Adapt<ApplicationUserDto>());

            config.NewConfig<ApplicationUser, ApplicationUserDto>()
                  .Map(dest => dest.UserFiles, src => src.UserFiles.Adapt<List<UserFileDto>>());
        }
    }

}
