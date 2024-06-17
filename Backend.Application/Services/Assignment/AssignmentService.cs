using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Exceptions;

namespace Backend.Application.Services.Assignment;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    public AssignmentService(IAssignmentRepository assignmentRepository, IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
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
}