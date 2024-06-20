using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services.AssignmentServices;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    private readonly IAssetRepository _assetRepository;
    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper, IAssetRepository assetRepository)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
        _assetRepository = assetRepository;
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
            var assignment = _mapper.Map<Assignment>(dto);
            assignment.CreatedBy = createName;
            assignment.CreatedAt = DateTime.Now;
            assignment.State = AssignmentState.Waiting;
            assignment.AssignedById = assignedById;
            await _assignmentRepository.InsertAsync(assignment);

            var assignedAsset = await _assetRepository.GetByIdAsync(assignment.AssetId) ?? throw new InvalidOperationException("Asset not found");
            assignedAsset.State = AssetState.Assigned;
            await _assetRepository.UpdateAsync(assignedAsset);

            var returnAssignment = await FindAssignmentByAssetIdAsync(assignment.AssetId);

            var res = _mapper.Map<AssignmentResponse>(returnAssignment);
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error {ex.Message}", ex);
        }
    }
}
