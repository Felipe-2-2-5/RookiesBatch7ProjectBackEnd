using Backend.Domain.Entities;
using Backend.Domain.Entity;
using Backend.Domain.Enum;

namespace Backend.Application.DTOs.AssetDTOs
{
    public class AssetResponseDTO
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string? Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public AssetState State { get; set; }
        public int AssignmentId { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
    }
}
