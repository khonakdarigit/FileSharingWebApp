using Application.DTOs;


namespace Application.Interface
{
    public interface IFileShareService
    {
        Task DeleteFileShare(FileShareDto item);
        Task<FileShareDto> AddFileShare(FileShareDto fileShareDto);
    }
}
