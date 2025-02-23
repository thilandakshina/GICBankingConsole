using GICBankingSystem.Core.API.Controllers;
using GICBankingSystem.Core.Application.Commands.ProcessTransaction;
using GICBankingSystem.Core.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GICBankingSystem.Core.API.Tests;

[TestFixture]
public class TransactionControllerTests : ControllerTestBase<TransactionController>
{
    protected override TransactionController CreateController(IMediator mediator)
    {
        return new TransactionController(mediator);
    }

    [Test]
    public async Task ProcessTransaction_Success_ReturnsOkResult()
    {
        // Arrange
        var command = CreateSampleCommand();
        var expectedResult = CreateSampleResult();
        _mediatorMock.Setup(m => m.Send(command, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.ProcessTransaction(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(200));

        _mediatorMock.Verify(m => m.Send(command, default), Times.Once);
    }

    [Test]
    public void ProcessTransaction_WhenExceptionOccurs_ThrowsException()
    {
        // Arrange
        var command = CreateSampleCommand();
        var expectedErrorMessage = "Invalid transaction";
        _mediatorMock.Setup(m => m.Send(command, default))
            .ThrowsAsync(new Exception(expectedErrorMessage));

        // Act & Assert
        var exception = Assert.ThrowsAsync<Exception>(async () =>
            await _controller.ProcessTransaction(command));

        Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
    }

    [Test]
    public async Task ProcessTransaction_ValidatesResponseMapping()
    {
        // Arrange
        var command = CreateSampleCommand();
        var expectedResult = CreateSampleResult();
        _mediatorMock.Setup(m => m.Send(command, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.ProcessTransaction(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);

        // Extract the response object using pattern matching
        Assert.That(okResult.Value, Is.Not.Null);
        var dict = okResult.Value.GetType().GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

        Assert.That(dict.ContainsKey("response"), Is.True, "Response property not found");
        var statement = dict["response"] as StatementDto;
        Assert.That(statement, Is.Not.Null, "Response is not a StatementDto");

        // Verify statement properties
        Assert.That(statement.AccountNo, Is.EqualTo("ACC001"));
        Assert.That(statement.StartingBalance, Is.EqualTo(50));
        Assert.That(statement.FinalBalance, Is.EqualTo(150));

        // Verify transactions
        Assert.That(statement.Transactions, Has.Count.EqualTo(1));
        var transaction = statement.Transactions[0];
        Assert.Multiple(() =>
        {
            Assert.That(transaction.TransactionId, Is.EqualTo("TR001"));
            Assert.That(transaction.Type, Is.EqualTo("D"));
            Assert.That(transaction.Amount, Is.EqualTo(100));
            Assert.That(transaction.Balance, Is.EqualTo(150));
        });
    }

    private static ProcessTransactionCommand CreateSampleCommand()
    {
        return new ProcessTransactionCommand(new TransactionDto
        {
            AccountNo = "ACC001",
            Type = "D",
            Amount = 100,
            CreatedDate = DateTime.Now
        });
    }

    private static ProcessTransactionResult CreateSampleResult()
    {
        return new ProcessTransactionResult(
            new StatementDto
            {
                AccountNo = "ACC001",
                StartingBalance = 50,
                FinalBalance = 150,
                Transactions = new List<StatementLineDto>
                {
                    new()
                    {
                        TransactionId = "TR001",
                        Type = "D",
                        Amount = 100,
                        Balance = 150,
                        CreatedDate = DateTime.Now
                    }
                }
            });
    }
}