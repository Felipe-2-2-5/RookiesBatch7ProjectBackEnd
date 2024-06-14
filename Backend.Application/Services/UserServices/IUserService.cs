using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Domain.Entity;

namespace Backend.Application.Services.UserServices
{
    public interface IUserService
    {
        Task<User?> FindUserByUserNameAsync(string email);

        Task<LoginResponse> LoginAsync(LoginDTO dto);

        Task<UserResponse> GetByIdAsync(int id);

        Task<bool> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);

        Task<PaginationResponse<UserResponse>> GetFilterAsync(UserFilterRequest request);

        Task<UserResponse> InsertAsync(UserDTO dto);
    }
}
