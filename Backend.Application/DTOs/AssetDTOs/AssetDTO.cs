using Backend.Application.Common.Converter;
using Backend.Domain.Enum;
using System.Text.Json.Serialization;

namespace Backend.Application.DTOs.AssetDTOs
{
    public class AssetDTO
    {
        public string AssetName { get; set; }
        public int CategoryId { get; set; }
        public string? Specification { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? InstalledDate { get; set; }
        public AssetState AssetState { get; set; }
    }
}
