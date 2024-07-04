using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Domain.Entities;
using Backend.Domain.Enum;

namespace Backend.Application.Services.AssignmentServices;

public interface IAssignmentService
{
    Task<AssignmentResponse> GetByIdAsync(int id);

    Task<PaginationResponse<AssignmentResponse>> GetFilterAsync(AssignmentFilterRequest request, Location location);

    Task<AssignmentResponse> InsertAsync(AssignmentDTO dto, string createName, int assignedById);

    Task<Assignment?> FindAssignmentByAssetIdAsync(int assetId);

    Task<AssignmentResponse> UpdateAsync(AssignmentDTO dto, int id, string modifiedName);

    Task DeleteAsync(int id);

    Task<PaginationResponse<AssignmentResponse>> GetMyAssignmentsAsync(MyAssignmentFilterRequest request);

    Task RespondAssignment(AssignmentRespondDto dto, int id);
    
}