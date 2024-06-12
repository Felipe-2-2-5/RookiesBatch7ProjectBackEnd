using Backend.Application.Common.Paging;
using Backend.Application.IRepositories;
using Backend.Domain.Entity;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Backend.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly AssetContext _assetContext;
        public UserRepository(AssetContext context) : base(context)
        {
            _assetContext = context;
        }
        public async Task<User?> FindUserByUserNameAsync(string email) => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == email);
        public async Task<User> GenerateUserInformation(User user)
        {
            int maxId = await _context.Users.MaxAsync(s => (int?)s.Id) ?? 0;
            string staffCode = $"SD{(maxId + 1).ToString("D4")}";

            // Generate Username
            string baseUsername = $"{user.FirstName.ToLower()}{string.Concat(user.LastName.ToLower().Split(' ').Select(w => w[0]))}"; string username = baseUsername;
            int usernameIndex = 1;
            while (await _context.Users.AnyAsync(s => s.UserName == username))
            {
                username = $"{baseUsername}{usernameIndex++}";
            }

            // Generate Password
            string password = $"{char.ToUpper(username[0])}{username.Substring(1)}@{user.DateOfBirth:ddMMyyyy}";
            user.Password = password;
            user.UserName = username;
            user.StaffCode = staffCode;

            return user;
        }
        public async Task<PaginationResponse<User>> GetFilterAsync(FilterRequest request)
        {
            IQueryable<User> query = _table;

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(p =>
                    p.StaffCode.Contains(request.SearchTerm) ||
                    p.UserName.Contains(request.SearchTerm));
            }

            if (request.SortOrder?.ToLower() == "descend")
            {
                query = query.OrderByDescending(GetSortProperty(request));
            }
            else
            {
                query = query.OrderBy(GetSortProperty(request));
            }
            var totalCount = await query.CountAsync();
            var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).AsNoTracking().ToListAsync();
            return new(items, totalCount);
        }
        private static Expression<Func<User, object>> GetSortProperty(FilterRequest request) =>
        request.SortColumn?.ToLower() switch
        {
            "code" => user => user.StaffCode,
            "name" => user => user.FirstName,
            "date" => user => user.JoinedDate,
            "type" => user => user.Type,
            _ => user => user.FirstName
        };
    }
}
