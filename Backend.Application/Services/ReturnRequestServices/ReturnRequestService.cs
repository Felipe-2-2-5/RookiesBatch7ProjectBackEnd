using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.ReturnRequestDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;

namespace Backend.Application.Services.ReturnRequestServices;
public class ReturnRequestService : IReturnRequestService
{
    private readonly IReturnRequestRepository _requestRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    private readonly IAssetRepository _assetRepository;

    public ReturnRequestService(IReturnRequestRepository requestRepository, IAssignmentRepository assignmentRepository, IMapper mapper, IAssetRepository assetRepository)
    {
        _requestRepository = requestRepository;
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
        _assetRepository = assetRepository;
    }

    public async Task<ReturnRequestResponse> GetByIdAsync(int id)
    {
        var request = await _requestRepository.GetByIdAsync(id) ?? throw new NotFoundException();
        var dto = _mapper.Map<ReturnRequestResponse>(request);
        return dto;
    }
    public async Task CreateRequest(int assignmentId, string createName, int createId, Role role)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        if (assignment!.AssignedToId != createId && role == Role.Staff)
        {
            throw new ForbiddenException("No Permission");
        }
        if (assignment.State == AssignmentState.WaitingForAcceptance)
        {
            throw new DataInvalidException("Assignment not accepted");
        }
        if (assignment!.IsDeleted == true)
        {
            throw new DataInvalidException("Asset has been returned");
        }
        if (assignment!.ReturnRequest != null)
        {
            throw new DataInvalidException("Assignment already has return request.");
        }
        var request = new ReturnRequest
        {
            AssignmentId = assignmentId,
            RequestorId = createId,
            State = ReturnRequestState.WaitingForReturning,
            CreatedBy = createName,
            CreatedAt = DateTime.Now
        };
        await _requestRepository.InsertAsync(request);

        assignment.State = AssignmentState.WaitingForReturning;
        await _assignmentRepository.UpdateAsync(assignment);
    }

    public async Task<PaginationResponse<ReturnRequestResponse>> GetFilterAsync(ReturnRequestFilterRequest request, Location location)
    {
        var res = await _requestRepository.GetFilterAsync(request, location);
        var dtos = _mapper.Map<IEnumerable<ReturnRequestResponse>>(res.Data);
        return new(dtos, res.TotalCount);
    }

    public async Task CancelRequestAsync(int id, string modifyName, Role role)
    {
        var request = await _requestRepository.GetByIdAsync(id) ?? throw new NotFoundException("Not found request.");
        if (role == Role.Staff)
        {
            throw new ForbiddenException("No Permission");
        }
        // Check if the return request state is completed
        if (request.State == ReturnRequestState.Completed)
        {
            throw new DataInvalidException("Cannot delete return request because its state is 'Completed'.");
        }

        request.ModifiedAt = DateTime.Now;
        request.ModifiedBy = modifyName;
        await _requestRepository.DeleteAsync(request);

        var assignment = await _assignmentRepository.FindAssignmentByIdWithoutAsset(request.AssignmentId) ?? throw new NotFoundException("Not found assignment");
        assignment.State = AssignmentState.Accepted;
        await _assignmentRepository.UpdateAsync(assignment);
    }

    public async Task CompleteRequestAsync(int id, int acceptedBy)
    {
        var request = await _requestRepository.GetByIdAsync(id) ?? throw new NotFoundException("Not found request.");

        request.State = ReturnRequestState.Completed;
        request.ReturnedDate = DateTime.Now;
        request.AcceptorId = acceptedBy;
        await _requestRepository.UpdateAsync(request);

        var assignment = await _assignmentRepository.FindAssignmentByIdWithoutAsset(request.AssignmentId) ?? throw new NotFoundException("Not found assignment");

        assignment.IsDeleted = true;
        request.Assignment!.Asset!.State = AssetState.Available;
        await _assignmentRepository.UpdateAsync(assignment);
    }
}
