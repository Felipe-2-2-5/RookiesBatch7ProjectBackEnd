using AutoMapper;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;

namespace Backend.Application.Services.AssetServices
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public AssetService(IAssetRepository assetRepository, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
        }
        public async Task InsertAsync(AssetDTO assetDTO, string createName, Location location)
        {
            var asset = _mapper.Map<Asset>(assetDTO);
            asset.CreatedAt = DateTime.Now;
            asset.CreatedBy = createName;
            asset.Location = location;
            await _assetRepository.GenerateAssetInfo(asset);
            await _assetRepository.InsertAsync(asset);
        }
        public async Task<AssetResponseDTO> GetByIdAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                throw new NotFoundException();
            }
            var dto = _mapper.Map<AssetResponseDTO>(asset);
            return dto;
        }
    }
}
