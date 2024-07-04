using Backend.Application.Common.Paging;
using Backend.Application.DTOs.ReturnRequestDTOs;
using Backend.Domain.Entities;
using Backend.Domain.Enum;

namespace Backend.Application.Services.ReturnRequestServices
{
    public interface IReturnRequestService
    {
        Task CreateRequest(int assignmentId, string createName, int createId, Role role);
        Task<ReturnRequestResponse> GetByIdAsync(int id);
        Task<PaginationResponse<ReturnRequestResponse>> GetFilterAsync(ReturnRequestFilterRequest request, Location location);

        Task DeleteAsync(int id);

        Task CompleteRequestAsync(int id, int UserId);
    }
}