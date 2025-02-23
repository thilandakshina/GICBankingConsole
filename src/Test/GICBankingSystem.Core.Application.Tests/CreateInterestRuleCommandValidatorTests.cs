using GICBankingSystem.Core.Application.Commands.CreateInterestRule;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Application.Tests.Base;

namespace GICBankingSystem.Core.Application.Tests;

[TestFixture]
public class CreateInterestRuleCommandValidatorTests : ValidatorTestBase<CreateInterestRuleCommandValidator>
{
    protected override CreateInterestRuleCommandValidator CreateValidator()
    {
        return new CreateInterestRuleCommandValidator();
    }

    [Test]
    public void Validate_EmptyRuleId_ShouldFail()
    {
        // Arrange
        var command = new CreateInterestRuleCommand(new InterestRuleDto
        {
            RuleId = "",
            Rate = 3.5m,
            EffectiveDate = DateTime.Now
        });

        // Act
        var result = Validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "InterestRule.RuleId"), Is.True);
    }

    [Test]
    public void Validate_ZeroRate_ShouldFail()
    {
        // Arrange
        var command = new CreateInterestRuleCommand(new InterestRuleDto
        {
            RuleId = "RULE001",
            Rate = 0,
            EffectiveDate = DateTime.Now
        });

        // Act
        var result = Validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "InterestRule.Rate"), Is.True);
    }

    [Test]
    public void Validate_NegativeRate_ShouldFail()
    {
        // Arrange
        var command = new CreateInterestRuleCommand(new InterestRuleDto
        {
            RuleId = "RULE001",
            Rate = -1.0m,
            EffectiveDate = DateTime.Now
        });

        // Act
        var result = Validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "InterestRule.Rate"), Is.True);
    }

    [Test]
    public void Validate_EmptyEffectiveDate_ShouldFail()
    {
        // Arrange
        var command = new CreateInterestRuleCommand(new InterestRuleDto
        {
            RuleId = "RULE001",
            Rate = 3.5m,
            EffectiveDate = default
        });

        // Act
        var result = Validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "InterestRule.EffectiveDate"), Is.True);
    }

    [Test]
    public void Validate_ValidCommand_ShouldPass()
    {
        // Arrange
        var command = new CreateInterestRuleCommand(new InterestRuleDto
        {
            RuleId = "RULE001",
            Rate = 3.5m,
            EffectiveDate = DateTime.Now
        });

        // Act
        var result = Validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }
}