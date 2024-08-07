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
public class AssignmentController : BaseController
{
    private readonly IAssignmentService _assignmentService;

    private int AssignedById
    {
        get
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new Exception("User ID is not a valid integer.");
        }
    }

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
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> GetFilterAsync(AssignmentFilterRequest request)
    {
        if (request.AssignedDate == DateTime.MinValue)
        {
            request.AssignedDate = DateTime.Today;
        }
        var res = await _assignmentService.GetFilterAsync(request, Location);
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

    // DELETE: api/assignments/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _assignmentService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("my-assignments")]
    [Authorize]
    public async Task<IActionResult> GetMyAssignmentsAsync(MyAssignmentFilterRequest request)
    {
        request.UserId = UserId;
        var res = await _assignmentService.GetMyAssignmentsAsync(request);
        return Ok(res);
    }

    [HttpPut("{id}/respond")]
    [Authorize]
    public async Task<IActionResult> AssignmentRespond(AssignmentRespondDto dto, int id)
    {
        await _assignmentService.RespondAssignment(dto, id);
        return Ok();
    }

}