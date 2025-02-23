using GICBankingSystem.Core.Application.Commands.CreateInterestRule;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Application.Interfaces.Services;
using GICBankingSystem.Core.Application.Tests.Base;
using Moq;

namespace GICBankingSystem.Core.Application.Tests;

[TestFixture]
public class CreateInterestRuleCommandHandlerTests : HandlerTestBase<CreateInterestRuleCommandHandler, IInterestRuleService>
{
    protected override CreateInterestRuleCommandHandler CreateHandler()
    {
        return new CreateInterestRuleCommandHandler(ServiceMock.Object);
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldReturnResult()
    {
        // Arrange
        var interestRule = new InterestRuleDto
        {
            RuleId = "RULE001",
            EffectiveDate = DateTime.Now,
            Rate = 3.5m
        };

        var command = new CreateInterestRuleCommand(interestRule);

        var expectedRules = new List<InterestRuleDto>
        {
            interestRule,
            new InterestRuleDto
            {
                RuleId = "RULE002",
                EffectiveDate = DateTime.Now.AddDays(1),
                Rate = 4.0m
            }
        };

        ServiceMock.Setup(s => s.CreateInterestRule(
            It.IsAny<InterestRuleDto>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRules);

        // Act
        var result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.InterestRules, Is.EqualTo(expectedRules));
        ServiceMock.Verify(s => s.CreateInterestRule(
            It.Is<InterestRuleDto>(dto => dto.RuleId == "RULE001"),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}