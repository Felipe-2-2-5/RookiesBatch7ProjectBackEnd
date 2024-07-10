using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.Services.AssetServices;
using Backend.Application.Services.ReportServices;
using Backend.Domain.Entities;
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

        [HttpPost("filter-choosing/{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> GetFilterChoosingAsync(int id, AssetFilterRequest request)
        {
            var res = await _assetService.GetFilterChoosingAsync(id, request, Location);
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
        [HttpPost("report")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<ActionResult<List<AssetReport>>> GetAssetReport(BaseFilterRequest filterDto)
        {
            var result = await _reportService.GetAssetReportAsync(filterDto.SortColumn, filterDto.SortOrder, filterDto.PageSize, filterDto.Page);
            return Ok(result);
        }

        [HttpPost("report/export")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> ExportAssetReport(BaseFilterRequest filterDto)
        {
            var fileContent = await _reportService.ExportAssetReportAsync(filterDto.SortColumn, filterDto.SortOrder);
            var currentDate = DateTime.Now.ToString("ddMMyyyy");
            var fileName = $"AssetReport_{currentDate}_RookiesTeam2.xlsx";

            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
