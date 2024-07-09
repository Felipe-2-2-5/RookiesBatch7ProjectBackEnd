using System.Globalization;
using Backend.Application.DTOs.AssetDTOs;
using Backend.Application.Validations;
using Backend.Domain.Enum;
using FluentValidation.TestHelper;

namespace Backend.Tests.ValidationTests
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
        public void Should_Have_Error_When_State_Is_Null()
        {
            var model = new AssetDTO { State = null };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(asset => asset.State);
        }

        [Test]
        public void Should_Not_Have_Error_When_InstalledDate_Is_Valid()
        {
            var model = new AssetDTO { InstalledDate = DateTime.ParseExact("12/12/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture) };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(asset => asset.InstalledDate);
        }
        
    }
}