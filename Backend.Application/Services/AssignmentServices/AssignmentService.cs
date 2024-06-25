using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;

namespace Backend.Application.Services.AssignmentServices;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    private readonly IAssetRepository _assetRepository;
    private readonly IUserRepository _userRepository;

    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper, IAssetRepository assetRepository, IUserRepository userRepository)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
        _assetRepository = assetRepository;
        _userRepository = userRepository;
    }

    public async Task<Assignment?> FindAssignmentByAssetIdAsync(int assetId)
    {
        return await _assignmentRepository.FindAssignmentByAssetIdAsync(assetId);
    }

    public async Task<AssignmentResponse> GetByIdAsync(int id)
    {
        var entity = await _assignmentRepository.GetByIdAsync(id) ?? throw new NotFoundException();
        var dto = _mapper.Map<AssignmentResponse>(entity);
        return dto;
    }

    public async Task<PaginationResponse<AssignmentResponse>> GetFilterAsync(AssignmentFilterRequest request)
    {
        var res = await _assignmentRepository.GetFilterAsync(request);
        var dto = _mapper.Map<IEnumerable<AssignmentResponse>>(res.Data);
        return new PaginationResponse<AssignmentResponse>(dto, res.TotalCount);
    }

    public async Task<AssignmentResponse> InsertAsync(AssignmentDTO dto, string createName, int assignedById)
    {
        try
        {
            var assignedAsset = await _assetRepository.GetByIdAsync(dto.AssetId) ?? throw new NotFoundException("Asset not found");

            //asset is already assigned
            if (assignedAsset.State == AssetState.Assigned)
            {
                var assignedAssignment = await _assignmentRepository.FindAssignmentByAssetIdAsync(dto.AssetId);
                var assignedUser = await _userRepository.GetByIdAsync(assignedAssignment.AssignedToId);
                throw new DataInvalidException($"Asset has been assigned to {assignedUser.UserName} ");
            }

            var assignment = _mapper.Map<Assignment>(dto);
            assignment.CreatedBy = createName;
            assignment.CreatedAt = DateTime.Now;
            assignment.State = AssignmentState.Waiting;
            assignment.AssignedById = assignedById;
            await _assignmentRepository.InsertAsync(assignment);

            assignedAsset.State = AssetState.Assigned;
            await _assetRepository.UpdateAsync(assignedAsset);

            var returnAssignment = await _assignmentRepository.FindLastestAssignment();

            var res = _mapper.Map<AssignmentResponse>(returnAssignment);
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error {ex.Message}", ex);
        }
    }

    public async Task<AssignmentResponse> UpdateAsync(AssignmentDTO dto, int id, string modifiedName)
    {
        try
        {
            var assignment = await _assignmentRepository.FindAssignmentByAssetIdWithoutAsset(id) ?? throw new NotFoundException("Not found assignment");
            if (assignment.State == AssignmentState.Accepted)
            {
                throw new DataInvalidException("Assignment is assigned to user");
            }

            var newAsset = await _assetRepository.GetByIdAsync(dto.AssetId) ?? throw new NotFoundException("Not found assignment");
            if (newAsset.State == AssetState.Assigned)
            {
                var assignedAssignment = await _assignmentRepository.FindAssignmentByAssetIdAsync(dto.AssetId);
                var assignedUser = await _userRepository.GetByIdAsync(assignedAssignment.AssignedToId);
                throw new DataInvalidException($"Asset has been assigned to {assignedUser.UserName} ");
            }

            var oldAsset = await _assetRepository.GetByIdAsync(assignment.AssetId) ?? throw new NotFoundException("Not found asset");
            oldAsset.Assignments = null;
            oldAsset.State = AssetState.Available;
            await _assetRepository.UpdateAsync(oldAsset);

            _mapper.Map(dto, assignment);
            assignment.ModifiedBy = modifiedName;
            assignment.ModifiedAt = DateTime.Now;
            await _assignmentRepository.UpdateAsync(assignment);

            newAsset.State = AssetState.Assigned;
            await _assetRepository.UpdateAsync(newAsset);

            return _mapper.Map<AssignmentResponse>(assignment);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error {ex.Message}", ex);
        }
    }
}
