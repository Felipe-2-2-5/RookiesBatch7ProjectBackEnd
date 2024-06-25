using AutoMapper;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Domain.Entities;

namespace Backend.Application.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserDTO, User>();
        }
    }
}
