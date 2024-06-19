using Backend.Domain.Enum;

namespace Backend.Application.DTOs.AssignmentDTOs
{
    public class AssignmentResponse
    {
        public int Id { get; set; }
        public int AssignedToId { get; set; }
        public int AssignedById { get; set; }
        public DateTime AssignedDate { get; set; }
        public AssignmentState State { get; set; }
        public int AssetId { get; set; }
        public string? Note { get; set; }
    }
}