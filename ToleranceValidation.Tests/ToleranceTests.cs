using NUnit.Framework;

namespace ToleranceValidation.Tests
{
    using System.Linq;
    using FluentAssertions;

    public class ToleranceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void WhenSingleRuleValidatorCreatedWithValidRules_ThenValidatorHasErrorsIsFalse()
        {
            // Arrange
            var firstFieldValue = 12;
            var secondFieldValue = 11;
            var dataValue = 1;
            var fieldName = "Excess Mileage tolerance";
            var message = "Excess mileage fee calculated outside of tolerance.";

            // Act
            var validator = new FieldValidator<decimal>(firstFieldValue, secondFieldValue, dataValue, fieldName)
                .AddRule(
                    (firstValue, secondValue, tolerance) => firstValue + tolerance >= secondValue, message);

            var validatorResult = validator.GetResult();

            // Assert
            validatorResult.HasErrors.Should().Be(false);
            validatorResult.ValidationErrors.Count.Should().Be(0);
            validatorResult.FieldName.Should().Be(fieldName);
        }

        [Test]
        public void WhenSingleRuleValidatorCreatedWithInvalidRules_ThenValidatorHasErrorsIsTrue()
        {
            // Arrange
            var firstFieldValue = 1;
            var secondFieldValue = 11;
            var dataValue = 1;
            var fieldName = "Excess Mileage";
            var message = "Excess mileage fee calculated outside of tolerance.";

            // Act
            var validator = new FieldValidator<decimal>(firstFieldValue, secondFieldValue, dataValue, fieldName)
                .AddRule(
                    (firstValue, secondValue, tolerance) => firstValue + tolerance >= secondValue, message);

            var validatorResult = validator.GetResult();

            // Assert
            validatorResult.HasErrors.Should().Be(true);
            validatorResult.ValidationErrors.Count.Should().Be(1);
            validatorResult.FieldName.Should().Be(fieldName);
            validatorResult.ValidationErrors.First().Item2.Should().Be(message);
        }
        
        [Test]
        public void WhenMultipleRuleValidatorCreatedWithValidRules_ThenValidatorHasErrorsIsFalse()
        {
            // Arrange
            var firstFieldValue = 12;
            var secondFieldValue = 11;
            var dataValue = 1;
            var fieldName = "Excess Mileage";
            var toleranceMessage = "Excess mileage fee calculated outside of tolerance.";
            var zeroCodeWeaversMessage = "CodeWeavers Excess mileage fee must be greater than zero.";
            var zeroPosApiMessage = "Pos API Excess mileage fee must be greater than zero.";

            // Act
            var fieldValidatorCollection = new FieldValidatorCollection
            {
                new FieldValidator<decimal>(firstFieldValue, secondFieldValue, dataValue, fieldName)
                    .AddRule((firstValue, secondValue, tolerance) => firstValue + tolerance >= secondValue, toleranceMessage),
                new FieldValidator<decimal>(firstFieldValue, secondFieldValue, dataValue, fieldName)
                    .AddRule((firstValue, minimum) => firstValue >= minimum, zeroCodeWeaversMessage)
                    .AddRule((secondValue, minimum) => secondValue >= minimum, zeroPosApiMessage)
            };

            // Assert
            fieldValidatorCollection.HasErrors.Should().Be(false);
            fieldValidatorCollection.ValidationErrors.Count.Should().Be(0);
        }

        [Test]
        public void WhenMultipleRuleValidatorCreatedWithInvalidRules_ThenValidatorHasErrorsIsTrue()
        {
            // Arrange
            var firstFieldValue = -150;
            var secondFieldValue = -11;
            var dataValue = 1;
            var fieldName = "Excess Mileage";
            var toleranceMessage = "Excess mileage fee calculated outside of tolerance.";
            var zeroCodeWeaversMessage = "CodeWeavers Excess mileage fee must be greater than zero.";
            var zeroPosApiMessage = "Pos API Excess mileage fee must be greater than zero.";

            // Act
            var validator = new FieldValidator<decimal>(firstFieldValue, secondFieldValue, dataValue, fieldName)
                            .AddRule((firstValue, secondValue, tolerance) => firstValue + tolerance >= secondValue, toleranceMessage)
                            .AddRule((firstValue, minimum) => firstValue >= minimum, zeroCodeWeaversMessage)
                            .AddRule((secondValue, minimum) => secondValue >= minimum, zeroPosApiMessage);

            var validatorResult = validator.GetResult();

            // Assert
            validator.GetResult().HasErrors.Should().Be(true);
            validatorResult.ValidationErrors.Count.Should().Be(3);
            validatorResult.FieldName.Should().Be(fieldName);
            validatorResult.ValidationErrors.First().Item2.Should().Be(toleranceMessage);
        }
    }
}