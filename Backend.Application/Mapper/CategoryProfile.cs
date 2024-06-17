using AutoMapper;
using Backend.Application.DTOs.CategoryDTOs;
using Backend.Domain.Entity;

namespace Backend.Application.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryResponseDTO>();
        }
    }
}
