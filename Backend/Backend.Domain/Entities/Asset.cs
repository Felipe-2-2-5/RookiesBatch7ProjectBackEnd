using Backend.Domain.Entity;
using Backend.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities
{
    public class Asset : BaseEntity
    {
        [StringLength(maximumLength: 6)]
        public string AssetCode { get; set; }

        [StringLength(maximumLength: 30)]
        public string AssetName { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [StringLength(maximumLength: 600)]
        public string? Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public AssigmentState State { get; set; }
    }
}
