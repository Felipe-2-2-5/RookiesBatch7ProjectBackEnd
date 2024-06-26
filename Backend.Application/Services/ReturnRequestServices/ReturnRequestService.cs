﻿using AutoMapper;
using Backend.Application.Common.Paging;
using Backend.Application.DTOs.ReturnRequestDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;

namespace Backend.Application.Services.ReturnRequestServices
{
    public class ReturnRequestService : IReturnRequestService
    {
        private readonly IReturnRequestRepository _requestRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public ReturnRequestService(IReturnRequestRepository requestRepository, IAssignmentRepository assignmentRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public async Task CreateRequest(int assignmentId, string createName, int createId, Role role)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
            if (assignment!.AssignedToId != createId && role == Role.Staff)
            {
                throw new ForbiddenException("No Permission");
            }
            if (assignment.State == AssignmentState.Waiting)
            {
                throw new DataInvalidException("Assignment not accepted");
            }
            if (assignment!.IsDeleted == true)
            {
                throw new DataInvalidException("Asset has been returned");
            }
            var request = new ReturnRequest();
            request.AssignmentId = assignmentId;
            request.RequestorId = createId;
            request.CreatedBy = createName;
            request.CreatedAt = DateTime.Now;
            await _requestRepository.InsertAsync(request);
        }

        public async Task<ReturnRequestResponse> GetByIdAsync(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id) ?? throw new NotFoundException();
            var dto = _mapper.Map<ReturnRequestResponse>(request);
            return dto;
        }

        public async Task<PaginationResponse<ReturnRequestResponse>> GetFilterAsync(ReturnRequestFilterRequest request, Location location)
        {
            var res = await _requestRepository.GetFilterAsync(request, location);
            var dtos = _mapper.Map<IEnumerable<ReturnRequestResponse>>(res.Data);
            return new(dtos, res.TotalCount);
        }
    }
}
