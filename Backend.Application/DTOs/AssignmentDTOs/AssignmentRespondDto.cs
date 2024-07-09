using Backend.Domain.Enum;

namespace Backend.Application.DTOs.AssignmentDTOs
{
    public class AssignmentRespondDto
    {
        public AssignmentState State { get; set; }
        public int AssignedToID { get; set; }
        public int AssetID { get; set; }
    }
}
