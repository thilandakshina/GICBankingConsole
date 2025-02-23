using GICBankingSystem.Core.Application.Commands.ProcessTransaction;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Application.Tests.Base;

namespace GICBankingSystem.Core.Application.Tests;
[TestFixture]
public class ProcessTransactionCommandValidatorTests : ValidatorTestBase<ProcessTransactionCommandValidator>
{
    protected override ProcessTransactionCommandValidator CreateValidator()
    {
        return new ProcessTransactionCommandValidator();
    }

    [Test]
    public void Validate_EmptyAccountNo_ShouldFail()
    {
        // Arrange
        var command = new ProcessTransactionCommand(new TransactionDto
        {
            AccountNo = "",
            Amount = 1000,
            Type = "D",
            CreatedDate = DateTime.Now
        });

        // Act
        var result = Validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "Transaction.AccountNo"), Is.True);
    }

    [TestCase("X")]
    [TestCase("")]
    public void Validate_InvalidTransactionType_ShouldFail(string type)
    {
        // Arrange
        var command = new ProcessTransactionCommand(new TransactionDto
        {
            AccountNo = "ACC001",
            Amount = 1000,
            Type = type,
            CreatedDate = DateTime.Now
        });

        // Act
        var result = Validator.Validate(command);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "Transaction.Type"), Is.True);
    }
}