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
        var entity = await _assignmentRepository.GetByIdAsync(id) ?? throw new NotFoundException("Assignment not found.");
        var dto = _mapper.Map<AssignmentResponse>(entity);
        return dto;
    }

    public async Task<PaginationResponse<AssignmentResponse>> GetFilterAsync(AssignmentFilterRequest request, Location location)
    {
        var res = await _assignmentRepository.GetFilterAsync(request, location);
        var dto = _mapper.Map<IEnumerable<AssignmentResponse>>(res.Data);
        return new PaginationResponse<AssignmentResponse>(dto, res.TotalCount);
    }

    public async Task<AssignmentResponse> InsertAsync(AssignmentDTO dto, string createName, int assignedById)
    {
        var assignedAsset = await _assetRepository.GetByIdAsync(dto.AssetId) ?? throw new NotFoundException(404, "Asset not found");
        var newUser = await _userRepository.GetByIdAsync(dto.AssignedToId) ?? throw new NotFoundException(404, "User not found");
        if (newUser.IsDeleted == true) 
        {
            throw new DataInvalidException("User has been disabled");
        }


        //asset is already assigned
        if (assignedAsset.State != AssetState.Available)
        {
            throw new DataInvalidException("Asset is not available");
        }

        var assignment = _mapper.Map<Assignment>(dto);
        assignment.CreatedBy = createName;
        assignment.CreatedAt = DateTime.Now;
        assignment.State = AssignmentState.WaitingForAcceptance;
        assignment.AssignedById = assignedById;
        await _assignmentRepository.InsertAsync(assignment);

        assignedAsset.State = AssetState.Assigned;
        assignedAsset.Assignments = null;
        await _assetRepository.UpdateAsync(assignedAsset);

        var returnAssignment = await _assignmentRepository.FindLatestAssignment();

        var res = _mapper.Map<AssignmentResponse>(returnAssignment);
        return res;

    }

    public async Task<AssignmentResponse> UpdateAsync(AssignmentDTO dto, int id, string modifiedName)
    {
        var assignment = await _assignmentRepository.FindAssignmentByIdWithoutAsset(id) ?? throw new NotFoundException("Not found assignment");
        
        if (assignment.State == AssignmentState.Accepted)
        {
            throw new DataInvalidException(400, "Assignment is assigned to user");
        }
        if (assignment.IsDeleted == true)
        {
            throw new DataInvalidException(400, "Assignment has been disabled");
        }
        // Change asset only
        if (assignment.AssignedToId == dto.AssignedToId && assignment.AssetId != dto.AssetId)
        {
            var newAsset = await _assetRepository.GetByIdAsync(dto.AssetId) ?? throw new NotFoundException("Not found new asset");
            if (newAsset.State != AssetState.Available)
            {
                throw new DataInvalidException("Asset is not available");
            }

            var oldAsset = await _assetRepository.GetByIdAsync(assignment.AssetId) ?? throw new NotFoundException("Not found old asset");
            oldAsset.Assignments = null;
            oldAsset.State = AssetState.Available;
            await _assetRepository.UpdateAsync(oldAsset);

            _mapper.Map(dto, assignment);
            assignment.ModifiedBy = modifiedName;
            assignment.ModifiedAt = DateTime.Now;
            await _assignmentRepository.UpdateAsync(assignment);

            newAsset.State = AssetState.Assigned;
            newAsset.Category = null;
            newAsset.Assignments = null;
            await _assetRepository.UpdateAsync(newAsset);

            return _mapper.Map<AssignmentResponse>(assignment);
        }
        //Change user only
        else if (assignment.AssignedToId != dto.AssignedToId && assignment.AssetId == dto.AssetId)
        {
            var newUser = await _userRepository.GetByIdAsync(dto.AssignedToId) ?? throw new NotFoundException(404, "Not found user");
            if (newUser.IsDeleted == true)
            {
                throw new DataInvalidException("User has been disabled");
            }

            var oldAsset = await _assetRepository.GetByIdAsync(assignment.AssetId);
            oldAsset!.Assignments = null;

            _mapper.Map(dto, assignment);
            assignment.Asset = oldAsset;
            assignment.AssignedTo = newUser;
            assignment.ModifiedBy = modifiedName;
            assignment.ModifiedAt = DateTime.Now;

            await _assignmentRepository.UpdateAsync(assignment);

            return _mapper.Map<AssignmentResponse>(assignment);
        }
        //Nothing Change
        else if (assignment.AssignedToId == dto.AssignedToId && assignment.AssetId == dto.AssetId)
        {
            var oldAsset = await _assetRepository.GetByIdAsync(assignment.AssetId) ?? throw new NotFoundException("Not found old asset");
            oldAsset.Assignments = null;

            _mapper.Map(dto, assignment);
            assignment.Asset = oldAsset;
            assignment.ModifiedBy = modifiedName;
            assignment.ModifiedAt = DateTime.Now;

            await _assignmentRepository.UpdateAsync(assignment);

            return _mapper.Map<AssignmentResponse>(assignment);
        }
        //Change user, change asset
        else
        {
            var newAsset = await _assetRepository.GetByIdAsync(dto.AssetId) ?? throw new NotFoundException("Not found new asset");
            if (newAsset.State == AssetState.Assigned)
            {
                throw new DataInvalidException("Asset has been assigned to other Staff");
            }
            newAsset.State = AssetState.Assigned;
            newAsset.Category = null;

            var oldAsset = await _assetRepository.GetByIdAsync(assignment.AssetId) ?? throw new NotFoundException("Not found old asset");
            oldAsset.Assignments = null;
            oldAsset.State = AssetState.Available;

            var newUser = await _userRepository.GetByIdAsync(dto.AssignedToId) ?? throw new NotFoundException(404, "Not found user");
            if (newUser.IsDeleted == true)
            {
                throw new DataInvalidException("User has been disabled");
            }

            _mapper.Map(dto, assignment);
            assignment.AssignedTo = newUser;
            assignment.ModifiedBy = modifiedName;
            assignment.ModifiedAt = DateTime.Now;

            await _assetRepository.UpdateAsync(oldAsset);
            await _assetRepository.UpdateAsync(newAsset);

            return _mapper.Map<AssignmentResponse>(assignment);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Assignment with id {id} not found.");

        // Check if the asset state is different than "Waiting for acceptance" or "Declined"
        if (assignment.State != AssignmentState.WaitingForAcceptance && assignment.State != AssignmentState.Declined)
        {
            throw new DataInvalidException("Cannot delete assignment because its state is not 'Waiting for acceptance' or 'Declined'.");
        }

        await _assignmentRepository.DeleteAsync(assignment);

        // Change the asset state back to "Available"
        var asset = _mapper.Map<Asset>(assignment.Asset!);
        asset!.State = AssetState.Available;
        await _assetRepository.UpdateAsync(asset);
    }

    public async Task<PaginationResponse<AssignmentResponse>> GetMyAssignmentsAsync(MyAssignmentFilterRequest request)
    {
        var res = await _assignmentRepository.GetMyAssignmentsAsync(request);
        var dto = _mapper.Map<IEnumerable<AssignmentResponse>>(res.Data);
        return new PaginationResponse<AssignmentResponse>(dto, res.TotalCount);
    }

    public async Task RespondAssignment(AssignmentRespondDto dto, int id)
    {
        var assignment = await _assignmentRepository.FindAssignmentByIdWithoutAsset(id) ?? throw new NotFoundException("Not found assignment");
        if (assignment.State != AssignmentState.WaitingForAcceptance)
        {
            throw new DataInvalidException("This assignment is already responded");
        }
        if (assignment.AssignedToId != dto.AssignedToID || assignment.AssetId != dto.AssetID)
        {
            throw new DataInvalidException("The assignment has been modified");
        }
        if (dto.State == AssignmentState.Accepted)
        {
            _mapper.Map(dto, assignment);
            await _assignmentRepository.UpdateAsync(assignment);
        }
        else if (dto.State == AssignmentState.Declined)
        {
            //Change Asset State back to "Available"
            var asset = await _assetRepository.GetByIdAsync(assignment.AssetId) ?? throw new NotFoundException("Not found asset");
            asset.Assignments = null;
            asset.State = AssetState.Available;
            await _assetRepository.UpdateAsync(asset);

            //Update Assignment State to "Decline"
            _mapper.Map(dto, assignment);
            await _assignmentRepository.UpdateAsync(assignment);
        }
        else
        {
            throw new DataInvalidException("State is not correct");
        }
    }
}
