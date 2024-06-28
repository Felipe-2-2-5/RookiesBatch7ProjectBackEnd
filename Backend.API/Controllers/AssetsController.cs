using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.Services.AssetServices;
using Backend.Application.Services.ReportServices;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetsController : BaseController
    {
        private readonly IAssetService _assetService;
        private readonly IReportService _reportService;


        public AssetsController(IAssetService assetService, IReportService reportService)
        {
            _assetService = assetService;
            _reportService = reportService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> InsertAsync(AssetDTO dto)
        {
            var res = await _assetService.InsertAsync(dto, UserName, Location);
            return Ok(res);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> UpdateAsync(int id, AssetDTO dto)
        {
            var res = await _assetService.UpdateAsync(id, dto, UserName);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var asset = await _assetService.GetByIdAsync(id);
            return Ok(asset);
        }

        [HttpPost("filter")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> GetFilterAsync(AssetFilterRequest request)
        {
            var res = await _assetService.GetFilterAsync(request, Location);
            return Ok(res);
        }

        // DELETE: api/asset/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _assetService.DeleteAsync(id);
            return NoContent();
        }

        //get report by category and state
        [HttpGet("report")]
        public async Task<ActionResult<List<AssetReportDto>>> GetAssetReport([FromQuery] string sortColumn, [FromQuery] string sortDirection)
        {
            var result = await _reportService.GetAssetReportAsync(sortColumn, sortDirection);
            return Ok(result);
        }
    }
}
