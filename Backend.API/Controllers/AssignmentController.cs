using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.Services.AssignmentServices;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[Route("api/assignments")]
[ApiController]
public class AssignmentController : BaseController
{
    private readonly IAssignmentService _assignmentService;
    public AssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var dto = await _assignmentService.GetByIdAsync(id);
        return Ok(dto);
    }

    [HttpPost("filter")]
    //[Authorize]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> GetFilterAsync(AssignmentFilterRequest request)
    {
        if (request.FromDate == DateTime.MinValue)
        {
            request.FromDate = DateTime.Today;
        }
        if (request.ToDate == DateTime.MinValue)
        {
            request.ToDate = DateTime.Today;
        }
        var res = await _assignmentService.GetFilterAsync(request);
        return Ok(res);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> InsertAsync(AssignmentDTO dto)
    {
        var res = await _assignmentService.InsertAsync(dto, UserName, UserId);
        return Ok(res);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> UpdateAsync(AssignmentDTO dto, int id)
    {
        var res = await _assignmentService.UpdateAsync(dto, id, UserName);
        return Ok(res);
    }
}