using Backend.Application.Common.Paging;
using Backend.Domain.Entities;
using Backend.Domain.Enum;

namespace Backend.Application.IRepositories;

public interface IAssignmentRepository : IBaseRepository<Assignment>
{
    Task<PaginationResponse<Assignment>> GetFilterAsync(AssignmentFilterRequest request, Location location);
    Task<Assignment?> FindAssignmentByAssetIdAsync(int assetCode);
    Task<Assignment?> FindLatestAssignment();
    Task<Assignment?> FindAssignmentByIdWithoutAsset(int id);
    Task<PaginationResponse<Assignment>> GetMyAssignmentsAsync(MyAssignmentFilterRequest request);
}