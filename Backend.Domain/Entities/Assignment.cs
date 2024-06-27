using Backend.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public int AssignedToId { get; set; }

        public int AssignedById { get; set; }

        public DateTime AssignedDate { get; set; }

        public AssignmentState State { get; set; }

        public int AssetId { get; set; }
        [StringLength(maximumLength: 600)]
        public string Note { get; set; }
        public Asset Asset { get; set; }

        public User AssignedTo { get; set; }

        public User AssignedBy { get; set; }
        public ReturnRequest ReturnRequest { get; set; }

    }
}
