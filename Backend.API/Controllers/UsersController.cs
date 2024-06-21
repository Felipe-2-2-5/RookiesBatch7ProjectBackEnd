﻿using Backend.Application.Common.Paging;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Application.Services.UserServices;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private string UserName => Convert.ToString(User.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        private Location Location => (Location)Enum.Parse(typeof(Location), User.Claims.First(c => c.Type == "Location").Value);
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var dto = await _userService.GetByIdAsync(id);
            return Ok(dto);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginDTO dto)
        {
            var result = await _userService.LoginAsync(dto);
            return Ok(result);
        }
        
        [HttpPost("change_password")]
        [Authorize]
        public async Task<ActionResult<LoginResponse>> ChangePasswordAsync(ChangePasswordDTO dto)
        {
            var result = await _userService.ChangePasswordAsync(dto);
            return Ok(result);
        }
        
        [HttpPost("filter")]
        [Authorize]
        public async Task<IActionResult> GetFilterAsync(UserFilterRequest request)
        {
            var res = await _userService.GetFilterAsync(request, Location);
            return Ok(res);
        }
        
        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> InsertAsync(UserDTO dto)
        {
            var res = await _userService.InsertAsync(dto, UserName);
            return Ok(res);
        }
        
        [HttpPut("disable/{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> DisableUserAsync(int id)
        {
            await _userService.DisableUserAsync(id);
            return Ok();
        }
    }

}
