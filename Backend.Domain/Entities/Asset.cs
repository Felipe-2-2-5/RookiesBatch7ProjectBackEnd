using Backend.Domain.Entity;
using Backend.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities
{
    public class Asset : BaseEntity
    {
        [StringLength(maximumLength: 10)]
        public string AssetCode { get; set; } = "";

        [StringLength(maximumLength: 50)]
        public string AssetName { get; set; } = "";
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [StringLength(maximumLength: 600)]
        public string? Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public AssetState State { get; set; }
        public Location Location { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
    }
}
