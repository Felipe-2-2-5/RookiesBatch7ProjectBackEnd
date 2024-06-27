using Backend.Application.DTOs.AssetDTOs;
using FluentValidation;
using System.Globalization;

namespace Backend.Application.Validations
{
    public class AssetValidator : AbstractValidator<AssetDTO>
    {
        public AssetValidator()
        {
            RuleFor(asset => asset.AssetName)
                .NotEmpty()
                .WithMessage("AssetName is required.")
                .Matches("^[a-zA-Z0-9 ]*$")
                .WithMessage("AssetName can only contain alphabets, numbers, and white spaces.")
                .MaximumLength(50)
                .WithMessage("AssetName must not exceed 50 characters.");

            RuleFor(asset => asset.Specification)
                .MaximumLength(500)
                .WithMessage("Specification must not exceed 500 characters.");

            RuleFor(asset => asset.InstalledDate)
                .NotEmpty()
                .WithMessage("InstalledDate is required.")
                .Must((dto, dob) => dob.HasValue && BeAValidDate(dob.Value)).WithMessage("Date of birth must be in the format dd/MM/yyyy.");
            ;

            RuleFor(asset => asset.State)
                .NotNull()
                .WithMessage("State is required.");

        }
        private bool BeAValidDate(DateTime date)
        {
            string dateString = date.ToString("dd/MM/yyyy");
            string[] formats = { "dd/MM/yyyy" };
            return DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }

}
