using Backend.Application.Common.Paging;
using Backend.Application.Services.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[Route("api/assignments")]
[ApiController]
public class AssignmentController : ControllerBase
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
}