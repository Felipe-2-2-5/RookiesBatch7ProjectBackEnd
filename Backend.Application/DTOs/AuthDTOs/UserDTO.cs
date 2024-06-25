using Backend.Application.Common.Converter;
using Backend.Domain.Enum;
using System.Text.Json.Serialization;


namespace Backend.Application.DTOs.AuthDTOs
{
    public class UserDTO
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? DateOfBirth { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? JoinedDate { get; set; }

        public Gender Gender { get; set; }

        public Role Type { get; set; }

        public Location Location { get; set; }

    }
}
