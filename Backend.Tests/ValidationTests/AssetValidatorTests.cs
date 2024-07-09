using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.Validations;
using FluentValidation.TestHelper;

namespace Backend.Application.Tests.Validations
{
    [TestFixture]
    public class AssetValidatorTests
    {
        private AssetValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new AssetValidator();
        }

        [Test]
        public void Should_Have_Error_When_AssetName_Is_Empty()
        {
            var model = new AssetDTO { AssetName = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(asset => asset.AssetName);
        }

        [Test]
        public void Should_Have_Error_When_AssetName_Exceeds_MaxLength()
        {
            var model = new AssetDTO { AssetName = new string('a', 51) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(asset => asset.AssetName);
        }

        [Test]
        public void Should_Have_Error_When_Specification_Exceeds_MaxLength()
        {
            var model = new AssetDTO { Specification = new string('a', 501) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(asset => asset.Specification);
        }

        [Test]
        public void Should_Have_Error_When_InstalledDate_Is_Empty()
        {
            var model = new AssetDTO { InstalledDate = null };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(asset => asset.InstalledDate);
        }

        [Test]
        public void Should_Have_Error_When_InstalledDate_Is_Invalid()
        {
            var model = new AssetDTO { InstalledDate = DateTime.Parse("01/01/2020") };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(asset => asset.InstalledDate);
        }

        [Test]
        public void Should_Have_Error_When_State_Is_Null()
        {
            var model = new AssetDTO { State = null };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(asset => asset.State);
        }
    }
}