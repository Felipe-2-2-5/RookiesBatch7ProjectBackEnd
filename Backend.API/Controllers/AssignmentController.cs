using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.Services.AssignmentServices;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.API.Controllers;

[Route("api/assignments")]
[ApiController]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private string UserName => Convert.ToString(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
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
    public async Task<IActionResult> InserAsync(AssignmentDTO dto)
    {
        var res = await _assignmentService.InsertAsync(dto, UserName);
        return Ok(res);
    }
}