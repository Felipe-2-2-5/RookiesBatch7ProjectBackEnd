using Backend.Application.DTOs.CategoryDTOs;
using Backend.Domain.Enum;

namespace Backend.Application.DTOs.AssetDTOs
{
    public class AssetResponseDTO
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public int CategoryId { get; set; }
        public CategoryResponseDTO Category { get; set; }
        public string? Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public AssetState State { get; set; }
        public int AssignmentId { get; set; }
    }
}
