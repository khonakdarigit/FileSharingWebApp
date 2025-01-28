using Application.DTOs;


namespace Application.Interface
{
    public interface IUserFileService
    {
        Task AddUserFileAsync(UserFileDto userFile);
        Task<IEnumerable<AccessFileDto>> AllFileShareWithMe(string userId);
        Task<UserFileDto> GetFileWithDetails(Guid id);
        Task<IEnumerable<UserFileDto>> GetUserFileWithDetailsByUserId(string userId);
        Task UpdateUserFileAsync(UserFileDto file);
        Task DeleteUserFileAsync(UserFileDto userFile);
    }
}
