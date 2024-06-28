using Backend.Application.DTOs.CategoryDTOs;
using FluentValidation;

namespace Backend.Application.Validations
{
    public class CategoryValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Prefix).NotEmpty().MaximumLength(4).Matches("^[a-zA-Z]*$");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).Matches("^[a-zA-Z ]*$");
        }
    }
}
