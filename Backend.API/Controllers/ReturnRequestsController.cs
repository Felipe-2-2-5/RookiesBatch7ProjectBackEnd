using Backend.Application.Services.ReturnRequestServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/return_requests")]
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
    }
}
