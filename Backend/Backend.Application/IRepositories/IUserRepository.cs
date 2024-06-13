using Backend.Application.Common.Paging;
using Backend.Domain.Entity;

namespace Backend.Application.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindUserByUserNameAsync(string userName);
        Task<User> GenerateUserInformation(User user);
        Task<PaginationResponse<User>> GetFilterAsync(UserFilterRequest request);
    }
}

