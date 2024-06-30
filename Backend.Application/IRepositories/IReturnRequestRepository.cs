using System.Linq.Expressions;
using Backend.Application.Common.Paging;
using Backend.Domain.Entities;
using Backend.Domain.Enum;

namespace Backend.Application.IRepositories
{
    public interface IReturnRequestRepository : IBaseRepository<ReturnRequest>
    {
        Task<PaginationResponse<ReturnRequest>> GetFilterAsync(ReturnRequestFilterRequest request, Location location);
    }
}