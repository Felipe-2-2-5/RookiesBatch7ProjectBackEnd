using Backend.Application.DTOs.CategoryDTOs;
using Backend.Application.Validations;
using FluentValidation.TestHelper;

namespace Backend.Tests.Validations
{
    [TestFixture]
    public class CategoryValidatorTests
    {
        private CategoryValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CategoryValidator();
        }

        [Test]
        public void Should_Have_Error_When_Prefix_Is_Empty()
        {
            var model = new CategoryDTO { Prefix = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Prefix);
        }

        [Test]
        public void Should_Have_Error_When_Prefix_Exceeds_MaxLength()
        {
            var model = new CategoryDTO { Prefix = "ABCDE" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Prefix);
        }

        [Test]
        public void Should_Have_Error_When_Prefix_Has_Invalid_Characters()
        {
            var model = new CategoryDTO { Prefix = "1234" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Prefix);
        }

        [Test]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new CategoryDTO { Name = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            var model = new CategoryDTO { Name = new string('A', 51) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Should_Have_Error_When_Name_Has_Invalid_Characters()
        {
            var model = new CategoryDTO { Name = "Name123" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Should_Not_Have_Error_When_Prefix_And_Name_Are_Valid()
        {
            var model = new CategoryDTO { Prefix = "ABCD", Name = "Valid Name" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Prefix);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }
    }
}