﻿using AutoMapper;
using Backend.Application.DTOs.CategoryDTOs;
using Backend.Application.IRepositories;
using Backend.Domain.Entity;
using Backend.Domain.Exceptions;
using FluentValidation;

namespace Backend.Application.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryDTO> _validator;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IValidator<CategoryDTO> validator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task InsertAsync(CategoryDTO dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new DataInvalidException(string.Join(", ", errors));
            }
            var existCategory = await _categoryRepository.FindCategoryByNameAsync(dto.Name);
            if (existCategory != null)
            {
                throw new DataInvalidException("Existing category name");
            }
            existCategory = await _categoryRepository.FindCategoryByPrefixAsync(dto.Prefix);
            if (existCategory != null)
            {
                throw new DataInvalidException("Existing category prefix");

            }
            try
            {
                var category = _mapper.Map<Category>(dto);
                await _categoryRepository.InsertAsync(category);
            }
            catch (Exception ex)
            {
                throw new DataInvalidException(ex.Message);
            }
        }
        public async Task<IEnumerable<CategoryResponseDTO>> GetFilterAsync(string? searchTerm)
        {
            var categories = await _categoryRepository.GetFilterAsync(searchTerm);
            var dtos = _mapper.Map<IEnumerable<CategoryResponseDTO>>(categories);
            return dtos;
        }

    }
}