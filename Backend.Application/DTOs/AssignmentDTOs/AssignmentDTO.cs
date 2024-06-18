using Backend.Domain.Enum;

namespace Backend.Application.DTOs.AssignmentDTOs
{
    public class AssignmentDTO
    {
        public int AssignedToId { get; set; }
        public int AssignedById { get; set; }
        public DateTime AssignedDate { get; set; }
        public AssignmentState State { get; set; }
        public int AssetId { get; set; }
    }
}