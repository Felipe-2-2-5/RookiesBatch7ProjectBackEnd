using AutoMapper;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Domain.Entities;

namespace Backend.Application.Mapper
{
    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            CreateMap<AssetDTO, Asset>();
            CreateMap<Asset, AssetResponseDTO>();
        }
    }
}
