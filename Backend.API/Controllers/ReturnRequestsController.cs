using Backend.Application.Common.Paging;
using Backend.Application.Services.ReturnRequestServices;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/return-requests")]
    [ApiController]
    public class ReturnRequestsController : BaseController
    {
        private readonly IReturnRequestService _requestService;

        public ReturnRequestsController(IReturnRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> InsertAsync(int id)
        {
            await _requestService.CreateRequest(id, UserName, UserId, Role);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var request = await _requestService.GetByIdAsync(id);
            return Ok(request);
        }

        [HttpPost("filter")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> GetFilterAsync(ReturnRequestFilterRequest request)
        {
            var res = await _requestService.GetFilterAsync(request, Location);
            return Ok(res);
        }
    }
}
