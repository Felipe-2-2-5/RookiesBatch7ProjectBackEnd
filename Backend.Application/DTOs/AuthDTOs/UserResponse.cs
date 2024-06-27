using Backend.Domain.Enum;

namespace Backend.Application.DTOs.AuthDTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string StaffCode { get; set; } = "";
        public string UserName { get; set; } = "";
        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public DateTime? DateOfBirth { get; set; }

        public DateTime? JoinedDate { get; set; }

        public Gender Gender { get; set; }

        public Role Type { get; set; }
        public Location Location { get; set; }
    }
}
