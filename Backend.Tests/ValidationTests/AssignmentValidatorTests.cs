using Backend.Application.DTOs.AssignmentDTOs;
using Backend.Application.Validations;
using FluentValidation.TestHelper;

namespace Backend.Application.Tests.Validations
{
    [TestFixture]
    public class AssignmentValidatorTests
    {
        private AssignmentValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new AssignmentValidator();
        }

        [Test]
        public void Should_Have_Error_When_AssignedToId_Is_Zero()
        {
            var model = new AssignmentDTO { AssignedToId = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AssignedToId);
        }

        [Test]
        public void Should_Have_Error_When_AssignedToId_Is_Negative()
        {
            var model = new AssignmentDTO { AssignedToId = -1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AssignedToId);
        }

        [Test]
        public void Should_Not_Have_Error_When_AssignedToId_Is_Positive()
        {
            var model = new AssignmentDTO { AssignedToId = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.AssignedToId);
        }

        [Test]
        public void Should_Have_Error_When_AssignedDate_Is_Empty()
        {
            var model = new AssignmentDTO { AssignedDate = null };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AssignedDate);
        }

        [Test]
        public void Should_Have_Error_When_AssignedDate_Is_Invalid()
        {
            var model = new AssignmentDTO { AssignedDate = DateTime.Parse("01/01/0001") };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AssignedDate);
        }

        [Test]
        public void Should_Have_Error_When_AssignedDate_Is_Not_Greater_Than_Today()
        {
            var model = new AssignmentDTO { AssignedDate = DateTime.Today };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AssignedDate);
        }

        [Test]
        public void Should_Not_Have_Error_When_AssignedDate_Is_Valid()
        {
            var model = new AssignmentDTO { AssignedDate = DateTime.Today.AddDays(1) };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.AssignedDate);
        }

        [Test]
        public void Should_Have_Error_When_AssetId_Is_Zero()
        {
            var model = new AssignmentDTO { AssetId = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AssetId);
        }

        [Test]
        public void Should_Have_Error_When_AssetId_Is_Negative()
        {
            var model = new AssignmentDTO { AssetId = -1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.AssetId);
        }

        [Test]
        public void Should_Not_Have_Error_When_AssetId_Is_Positive()
        {
            var model = new AssignmentDTO { AssetId = 1 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.AssetId);
        }

        [Test]
        public void Should_Have_Error_When_Note_Exceeds_Maximum_Length()
        {
            var model = new AssignmentDTO { Note = new string('a', 601) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Note);
        }

        [Test]
        public void Should_Not_Have_Error_When_Note_Is_Within_Maximum_Length()
        {
            var model = new AssignmentDTO { Note = new string('a', 600) };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Note);
        }
    }
}