using Backend.Application.Common.Converter;
using Backend.Domain.Enum;
using System.Text.Json.Serialization;
namespace Backend.Application.DTOs.AssignmentDTOs
{
    public class AssignmentDTO
    {
        public int AssignedToId { get; set; }
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? AssignedDate { get; set; }
        public int AssetId { get; set; }
        public string Note { get; set; }
    }
}