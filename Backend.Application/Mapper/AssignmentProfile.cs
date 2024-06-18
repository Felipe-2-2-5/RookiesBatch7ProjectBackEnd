using AutoMapper;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Domain.Entities;

namespace Backend.Application.Mapper;

public class AssignmentProfile : Profile
{
    public AssignmentProfile()
    {
        CreateMap<Assignment, AssignmentResponse>();
        CreateMap<AssignmentDTO, Assignment>();
    }
}