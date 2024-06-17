using Backend.Application.DTOs.AuthDTOs;
using Backend.Domain.Enum;
using FluentValidation;
using System.Globalization;

namespace Backend.Application.Validations;

public class UserValidator : AbstractValidator<UserDTO>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Matches("^[a-zA-Z]+$").WithMessage("First name must contain only alphabetical characters.")
            .MaximumLength(20).WithMessage("First name must not exceed 20 characters.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Matches("^[a-zA-Z\\s]+$").WithMessage("Last name must contain only alphabetical characters and spaces.")
            .MaximumLength(20).WithMessage("Last name must not exceed 20 characters.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .Must((dto, dob) => dob.HasValue && BeAValidDate(dob.Value)).WithMessage("Date of birth must be in the format dd/MM/yyyy.")
            .Must(dob => dob.HasValue && IsOlderThan(dob.Value, 18)).WithMessage("User must be at least 18 years old.");

        RuleFor(x => x.JoinedDate)
            .NotEmpty().WithMessage("Joined date is required.")
            .Must((dto, date) => date.HasValue && BeAValidDate(date.Value)).WithMessage("Joined date must be in the format dd/MM/yyyy.")
            .Must(date => date.HasValue && date.Value.DayOfWeek != DayOfWeek.Saturday && date.Value.DayOfWeek != DayOfWeek.Sunday).WithMessage("Joined date must not be on a weekend.")
            .GreaterThan(x => x.DateOfBirth).WithMessage("Joined date must be after date of birth.");

        RuleFor(x => x.Gender)
            .NotNull().WithMessage("Please enter Gender")
            .Must(gender => Enum.IsDefined(typeof(Gender), gender)).WithMessage("Invalid gender.");

        RuleFor(x => x.Type)
            .NotNull().WithMessage("Please enter Role")
            .Must(role => Enum.IsDefined(typeof(Role), role)).WithMessage("Invalid role.");

        RuleFor(x => x.Location)
            .NotNull().WithMessage("Please enter Location")
            .Must(location => Enum.IsDefined(typeof(Location), location)).WithMessage("Invalid location.");
    }
    private bool BeAValidDate(DateTime date)
    {
        string dateString = date.ToString("dd/MM/yyyy");
        string[] formats = { "dd/MM/yyyy" };
        return DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
    private bool IsOlderThan(DateTime dob, int age)
    {
        var today = DateTime.Today;
        var calculatedAge = today.Year - dob.Year;
        if (dob.Date > today.AddYears(-calculatedAge)) calculatedAge--;
        return calculatedAge >= age;
    }
}