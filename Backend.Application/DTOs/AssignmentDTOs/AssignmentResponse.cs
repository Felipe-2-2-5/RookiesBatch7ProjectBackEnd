using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Domain.Enum;

namespace Backend.Application.DTOs.AssignmentDTOs
{
    public class AssignmentResponse
    {
        public int Id { get; set; }

        public int AssignedToId { get; set; }

        public DateTime AssignedDate { get; set; }

        public AssignmentState State { get; set; }

        public string? Note { get; set; }

        public UserResponse? AssignedTo { get; set; }

        public UserResponse? AssignedBy { get; set; }

        public AssetResponse? Asset { get; set; }
        public int AssetId { get; set; }
    }
}