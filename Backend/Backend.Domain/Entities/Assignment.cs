using Backend.Domain.Entity;

namespace Backend.Domain.Entities
{
    public class Assignment : BaseEntity
    {

        public int UserId { get; set; }
        public User User { get; set; }

        public int AssetId { get; set; }
        public Asset Asset { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime? ReturnedDate { get; set; }

    }
}
