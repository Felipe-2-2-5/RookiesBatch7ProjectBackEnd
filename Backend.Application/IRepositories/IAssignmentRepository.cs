using Backend.Application.Common.Paging;
using Backend.Domain.Entities;

namespace Backend.Application.IRepositories;

public interface IAssignmentRepository : IBaseRepository<Assignment>
{
    Task<PaginationResponse<Assignment>> GetFilterAsync(AssignmentFilterRequest request);
    Task<Assignment?> FindAssignmentByAssetIdAsync(int assetCode);
    Task<Assignment?> FindLastestAssignment();
}