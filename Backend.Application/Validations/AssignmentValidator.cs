using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Domain.Enum;
using FluentValidation;


namespace Backend.Application.Validations
{
    public class AssignmentValidator : AbstractValidator<AssignmentDTO>
    {
        public AssignmentValidator()
        {
            RuleFor(x => x.AssignedToId)
                .GreaterThan(0).WithMessage("AssignedToId must be a positive integer.")
                .NotEmpty().WithMessage("AssignedToId is required.");

            RuleFor(x => x.AssignedDate)
                .NotEmpty().WithMessage("Assigned date is required.")
                .Must(BeAValidDate).WithMessage("Assigned date must be in the format dd/MM/yyyy.")
                .GreaterThan(DateTime.Today).WithMessage("Assigned date must be greater than today.");

            RuleFor(x => x.AssetId)
                .GreaterThan(0).WithMessage("AssetId must be a positive integer.")
                .NotEmpty().WithMessage("AssetId is required.");

            RuleFor(x => x.Note)
                .MaximumLength(600).WithMessage("Note's content must not exceed 600 characters.");
        }

        private bool BeAValidDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return false;
            }

            string dateString = date.Value.ToString("dd/MM/yyyy");
            string[] formats = { "dd/MM/yyyy" };
            return DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}