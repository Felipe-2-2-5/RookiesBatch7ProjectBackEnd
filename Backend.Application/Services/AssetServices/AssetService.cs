using AutoMapper;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Domain.Enum;
using Backend.Domain.Exceptions;
using FluentValidation;

namespace Backend.Application.Services.AssetServices
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AssetDTO> _validator;

        public AssetService(IAssetRepository assetRepository, IMapper mapper, IValidator<AssetDTO> validator)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<AssetResponseDTO> InsertAsync(AssetDTO assetDTO, string createName, Location location)
        {
            var validationResult = await _validator.ValidateAsync(assetDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new DataInvalidException(string.Join(", ", errors));
            }
            var asset = _mapper.Map<Asset>(assetDTO);
            asset.CreatedAt = DateTime.Now;
            asset.CreatedBy = createName;
            asset.Location = location;
            await _assetRepository.GenerateAssetInfo(asset);
            await _assetRepository.InsertAsync(asset);
            asset = await _assetRepository.FindAssetByCodeAsync(asset.AssetCode);
            var dto = _mapper.Map<AssetResponseDTO>(asset);
            return dto;
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
