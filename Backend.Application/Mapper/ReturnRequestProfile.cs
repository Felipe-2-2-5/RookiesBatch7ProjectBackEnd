using AutoMapper;
using Backend.Application.DTOs.ReturnRequestDTOs;
using Backend.Domain.Entities;

namespace Backend.Application.Mapper
{
    public class ReturnRequestProfile : Profile
    {
        public ReturnRequestProfile()
        {
            CreateMap<ReturnRequestDTO, ReturnRequest>();
            CreateMap<ReturnRequest, ReturnRequestResponse>();
        }
    }
}
