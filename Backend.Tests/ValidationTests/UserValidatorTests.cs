using System.Globalization;
using Backend.Application.DTOs.AuthDTOs;
using Backend.Application.Validations;
using Backend.Domain.Enum;
using FluentValidation.TestHelper;

namespace Backend.Application.Tests.Validations
{
    [TestFixture]
    public class UserValidatorTests
    {
        private UserValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UserValidator();
        }

        [Test]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            var model = new UserDTO { FirstName = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.FirstName);
        }

        [Test]
        public void Should_Have_Error_When_LastName_Is_Empty()
        {
            var model = new UserDTO { LastName = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.LastName);
        }
        
        [Test]
        public void Should_Have_Error_When_User_Is_Just_Under_18()
        {
            var model = new UserDTO { DateOfBirth = DateTime.Today.AddYears(-18).AddDays(1) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.DateOfBirth);
        }

        [Test]
        public void Should_Have_Error_When_JoinedDate_Is_Weekend()
        {
            var model = new UserDTO { JoinedDate = new DateTime(2023, 10, 7) }; // Saturday
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.JoinedDate);
        }

        [Test]
        public void Should_Have_Error_When_JoinedDate_Is_Before_DateOfBirth()
        {
            var model = new UserDTO { DateOfBirth = new DateTime(2000, 1, 1), JoinedDate = new DateTime(1999, 1, 1) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.JoinedDate);
        }

        [Test]
        public void Should_Have_Error_When_Gender_Is_Invalid()
        {
            var model = new UserDTO { Gender = (Gender)999 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Gender);
        }

        [Test]
        public void Should_Have_Error_When_Type_Is_Invalid()
        {
            var model = new UserDTO { Type = (Role)999 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Type);
        }

        [Test]
        public void Should_Have_Error_When_Location_Is_Invalid()
        {
            var model = new UserDTO { Location = (Location)999 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Location);
        }
    }
}