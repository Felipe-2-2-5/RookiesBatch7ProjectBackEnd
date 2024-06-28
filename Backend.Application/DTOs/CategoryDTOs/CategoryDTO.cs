using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs.CategoryDTOs
{
    public class CategoryDTO
    {
        [StringLength(maximumLength: 4)]
        public string Prefix { get; set; } = "";

        [StringLength(maximumLength: 50)]
        public string Name { get; set; } = "";
    }
}
