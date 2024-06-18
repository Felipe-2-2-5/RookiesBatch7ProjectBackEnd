using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;

namespace Backend.Application.Services.AssignmentServices;

public interface IAssignmentService
{
    Task<AssignmentResponse> GetByIdAsync(int id);

    Task<PaginationResponse<AssignmentResponse>> GetFilterAsync(AssignmentFilterRequest request);
}