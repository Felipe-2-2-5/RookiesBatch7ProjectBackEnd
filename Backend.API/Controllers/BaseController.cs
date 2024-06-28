using Backend.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public string UserName => Convert.ToString(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        public Location Location => (Location)Enum.Parse(typeof(Location), User.Claims.First(c => c.Type == "Location").Value);
        public int UserId => Int16.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        public Role Role => (Role)Enum.Parse(typeof(Role), User.Claims.First(c => c.Type == ClaimTypes.Role).Value);

    }
}
