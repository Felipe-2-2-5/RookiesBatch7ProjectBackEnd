using System.Linq.Expressions;
using Backend.Application.Common.Paging;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AssetContext context) : base(context)
        {
        }
        public async Task<User?> FindUserByUserNameAsync(string email) => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == email);
        public async Task<User> GenerateUserInformation(User user)
        {
            int maxId = await _table.CountAsync();
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
        public async Task<PaginationResponse<User>> GetFilterAsync(UserFilterRequest request)
        {
            IQueryable<User> query = _table;

            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                if (request.Type == "Admin")
                {
                    query = query.Where(p => p.Type == Role.Admin);
                }
                else
                {
                    query = query.Where(p => p.Type == Role.Staff);

                }
            }
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(p =>
                    p.StaffCode.Contains(request.SearchTerm) ||
                    (p.FirstName + " " + p.LastName).Contains(request.SearchTerm));
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
        private static Expression<Func<User, object>> GetSortProperty(UserFilterRequest request) =>
        request.SortColumn?.ToLower() switch
        {
            "code" => user => user.StaffCode,
            "name" => user => user.FirstName + " " + user.LastName,
            "date" => user => user.JoinedDate,
            "type" => user => user.Type,
            _ => user => user.FirstName + " " + user.LastName
        };
    }
}
