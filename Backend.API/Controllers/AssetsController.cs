using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.Services.AssetServices;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;
        private string UserName => Convert.ToString(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        private Location Location => (Location)Enum.Parse(typeof(Location), User.Claims.First(c => c.Type == "Location").Value);
        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }
        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> InsertAsync(AssetDTO dto)
        {
            await _assetService.InsertAsync(dto, UserName, Location);
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var asset = await _assetService.GetByIdAsync(id);
            return Ok(asset);
        }
    }
}
