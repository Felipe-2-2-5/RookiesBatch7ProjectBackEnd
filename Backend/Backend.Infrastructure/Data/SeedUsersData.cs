using Backend.Domain.Entity;
using Backend.Domain.Enum;

namespace Backend.Infrastructure.Data
{
    internal class SeedUsersData
    {
        public List<User> GenerateSeedData()
        {
            var seedData = new List<User>
        {
            new() {
                Id = 1,
                StaffCode = "SD0001",
                UserName = "john_d",
                Password = "Password@123",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1985, 5, 15),
                JoinedDate = new DateTime(2020, 1, 1),
                Gender = Gender.Male,
                Type = Role.Admin,
                Location = Location.HaNoi,
                FirstLogin = true
            },
            new ()
            {
                Id = 2,
                StaffCode = "SD0002",
                UserName = "jane_s",
                Password = "Password@456",
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1990, 8, 25),
                JoinedDate = new DateTime(2019, 3, 10),
                Gender = Gender.Female,
                Type = Role.Staff,
                Location = Location.HoChiMinh,
                FirstLogin = true
            },
            new() {
                Id = 3,
                StaffCode = "SD0003",
                UserName = "michael_b",
                Password = "Password@789",
                FirstName = "Michael",
                LastName = "Brown",
                DateOfBirth = new DateTime(1975, 12, 5),
                JoinedDate = new DateTime(2018, 6, 20),
                Gender = Gender.Male,
                Type = Role.Admin,
                Location = Location.HaNoi,
                FirstLogin = true
            },
            new() {
                Id = 4,
                StaffCode = "SD0004",
                UserName = "emily_j",
                Password = "Password@101",
                FirstName = "Emily",
                LastName = "Jones",
                DateOfBirth = new DateTime(1988, 3, 30),
                JoinedDate = new DateTime(2021, 4, 12),
                Gender = Gender.Female,
                Type = Role.Staff,
                Location = Location.HaNoi,
                FirstLogin = true
            },
            new() {
                Id = 5,
                StaffCode = "SD0005",
                UserName = "david_w",
                Password = "Password@202",
                FirstName = "David",
                LastName = "Williams",
                DateOfBirth = new DateTime(1995, 7, 14),
                JoinedDate = new DateTime(2017, 9, 18),
                Gender = Gender.Male,
                Type = Role.Staff,
                Location = Location.HoChiMinh,
                FirstLogin = true
            }
        };

            return seedData;
        }
    }
}
