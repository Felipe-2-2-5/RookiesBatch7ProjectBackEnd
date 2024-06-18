using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Services.AssignmentServices;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
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

    public async Task<AssignmentResponse> InsertAsync(AssignmentDTO dto, string createName)
    {
        //try
        //{
        //    var assignment = _mapper.Map<Assignment>(dto);
        //    assignment.CreatedBy = createName;
        //    assignment.CreatedAt = DateTime.Now;
        //    await _assignmentRepository.InsertAsync(assignment);

        //    var returnAssignment = await FindAssignmentByAssetIdAsync(assignment.AssetId);
        //    var res = _mapper.Map<AssignmentResponse>(returnAssignment);
        //    return res;
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception($"An error occurred while create new assignment: {ex.Message}", ex);
        //}

        try
        {
            var assignment = _mapper.Map<Assignment>(dto);
            assignment.CreatedBy = createName;
            assignment.CreatedAt = DateTime.Now;
            await _assignmentRepository.InsertAsync(assignment);

            var returnAssignment = await FindAssignmentByAssetIdAsync(assignment.AssetId);
            var res = _mapper.Map<AssignmentResponse>(returnAssignment);
            return res;
        }
        catch (DbUpdateException dbEx)
        {
            // Log the detailed error message
            var errorMessage = dbEx.InnerException?.Message ?? dbEx.Message;
            throw new Exception($"An error occurred while saving the entity changes: {errorMessage}", dbEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while create new assignment: {ex.Message}", ex);
        }
    }
}