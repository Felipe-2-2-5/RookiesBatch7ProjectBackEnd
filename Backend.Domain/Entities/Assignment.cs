using Backend.Domain.Enum;

namespace Backend.Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public int AssignedToId { get; set; }
        
        public int AssignedById { get; set; }
        
        public DateTime AssignedDate { get; set; }
        
        public AssignmentState State { get; set; }

        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        
        public User AssignedTo { get; set; }
        
        public User AssignedBy { get; set; }
    }
}
