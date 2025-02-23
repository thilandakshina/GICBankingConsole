using GICBankingSystem.Core.Application.Commands.ProcessTransaction;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Application.Interfaces.Services;
using GICBankingSystem.Core.Application.Tests.Base;
using Moq;

namespace GICBankingSystem.Core.Application.Tests;
[TestFixture]
public class ProcessTransactionCommandHandlerTests : HandlerTestBase<ProcessTransactionCommandHandler, ITransactionService>
{
    protected override ProcessTransactionCommandHandler CreateHandler()
    {
        return new ProcessTransactionCommandHandler(ServiceMock.Object);
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldReturnResult()
    {
        // Arrange
        var command = new ProcessTransactionCommand(new TransactionDto
        {
            AccountNo = "ACC001",
            Amount = 1000,
            Type = "D",
            CreatedDate = DateTime.Now
        });

        var expectedStatement = new StatementDto
        {
            AccountNo = "ACC001",
            StartingBalance = 0,
            FinalBalance = 1000,
            Transactions = new List<StatementLineDto>()
        };

        ServiceMock.Setup(s => s.ProcessTransaction(
            It.IsAny<TransactionDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedStatement);

        // Act
        var result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Statement.AccountNo, Is.EqualTo("ACC001"));
    }
}