using Backend.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities
{
    public class User : BaseEntity
    {
        [StringLength(maximumLength: 8)]
        public string StaffCode { get; set; }

        [StringLength(maximumLength: 20)]
        public string UserName { get; set; }

        [StringLength(maximumLength: 200)]
        public string Password { get; set; }

        [StringLength(maximumLength: 20)]
        public string FirstName { get; set; }

        [StringLength(maximumLength: 40)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime JoinedDate { get; set; }

        public Gender Gender { get; set; }

        public Role Type { get; set; }

        [StringLength(maximumLength: 30)]
        public Location Location { get; set; }
        public bool FirstLogin { get; set; } = true;

    }
}
