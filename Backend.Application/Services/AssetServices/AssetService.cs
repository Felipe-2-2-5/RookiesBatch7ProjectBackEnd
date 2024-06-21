using AutoMapper;
using Backend.Application.Common.Paging;
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
            try
            {
                await _assetRepository.InsertAsync(asset);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            asset = await _assetRepository.FindAssetByCodeAsync(asset.AssetCode);
            var dto = _mapper.Map<AssetResponseDTO>(asset);
            return dto;
        }
        public async Task<AssetResponseDTO> UpdateAsync(int id, AssetDTO assetDTO, string updatedName)
        {
            var validationResult = await _validator.ValidateAsync(assetDTO);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new DataInvalidException(string.Join(", ", errors));
            }
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
            {
                throw new NotFoundException();
            }
            {
                asset.AssetName = assetDTO.AssetName;
                asset.Specification = assetDTO.Specification;
                asset.InstalledDate = (DateTime)assetDTO.InstalledDate;
                asset.State = assetDTO.AssetState;
                asset.ModifiedBy = updatedName;
                asset.ModifiedAt = DateTime.Now;
                asset.Category = null;
                asset.Assignments = null;
                try
                {
                    await _assetRepository.UpdateAsync(asset);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
            var assetDto = _mapper.Map<AssetResponseDTO>(asset);
            return assetDto;
        }
        public async Task<AssetResponseDTO> GetByIdAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id) ?? throw new NotFoundException();
            var dto = _mapper.Map<AssetResponseDTO>(asset);
            return dto;
        }

        public async Task<PaginationResponse<AssetResponseDTO>> GetFilterAsync(AssetFilterRequest request, Location location)
        {
            var res = await _assetRepository.GetFilterAsync(request, location);
            var dtos = _mapper.Map<IEnumerable<AssetResponseDTO>>(res.Data);
            return new(dtos, res.TotalCount);
        }
    }
}
