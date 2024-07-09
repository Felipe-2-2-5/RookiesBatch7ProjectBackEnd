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
        public async Task<AssetResponse> InsertAsync(AssetDTO assetDTO, string createName, Location location)
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
            var dto = _mapper.Map<AssetResponse>(asset);
            return dto;
        }
        public async Task<AssetResponse> UpdateAsync(int id, AssetDTO assetDTO, string updatedName)
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
            var category = asset.Category;

            asset.AssetName = assetDTO.AssetName;
            asset.Specification = assetDTO.Specification;
            asset.InstalledDate = (DateTime)assetDTO.InstalledDate!;
            asset.State = assetDTO.State ?? 0;
            asset.ModifiedBy = updatedName;
            asset.ModifiedAt = DateTime.Now;
            asset.Category = null;
            asset.Assignments = null;

            await _assetRepository.UpdateAsync(asset);

            asset.Category = category;
            var assetDto = _mapper.Map<AssetResponse>(asset);
            return assetDto;
        }
        public async Task<AssetResponse> GetByIdAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id) ?? throw new NotFoundException();
            var dto = _mapper.Map<AssetResponse>(asset);
            return dto;
        }

        public async Task<PaginationResponse<AssetResponse>> GetFilterAsync(AssetFilterRequest request, Location location)
        {
            var res = await _assetRepository.GetFilterAsync(request, location);
            var dtos = _mapper.Map<IEnumerable<AssetResponse>>(res.Data);
            return new(dtos, res.TotalCount);
        }

        public async Task DeleteAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Asset with id {id} not found.");

            // Check if the asset state is assigned
            if (asset.State == AssetState.Assigned)
            {
                throw new DataInvalidException("Cannot delete asset because its state is 'Assigned'.");
            }

            // Check if the asset has assignments
            if (asset.Assignments != null && asset.Assignments.Count != 0)
            {
                throw new DataInvalidException("Cannot delete asset because it has assignments.");
            }

            await _assetRepository.DeleteAsync(asset);
        }
    }
}
