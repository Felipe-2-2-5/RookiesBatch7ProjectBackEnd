﻿using Backend.Application.DTOs.CategoryDTOs;
using Backend.Application.Services.CategoryServices;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFilterAsync([FromBody] string? searchTerm)
        {
            var res = await _categoryService.GetFilterAsync(searchTerm);
            return Ok(res);
        }
        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> InsertAsync(CategoryDTO dto)
        {
            await _categoryService.InsertAsync(dto);
            return Ok();
        }
    }
}