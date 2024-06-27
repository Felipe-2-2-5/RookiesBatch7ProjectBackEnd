using Backend.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entity
{
    public class Category : BaseEntity
    {
        [StringLength(maximumLength: 4)]
        public string Prefix { get; set; } = "";

        [StringLength(maximumLength: 50)]
        public string Name { get; set; } = "";

        public ICollection<Asset>? Assets { get; set; }
    }
}
