using GICBankingSystem.Core.API.Controllers;
using GICBankingSystem.Core.Application.Commands.CreateInterestRule;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Application.Queries.GetStatementQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GICBankingSystem.Core.API.Tests;

[TestFixture]
public class InterestControllerTests : ControllerTestBase<InterestController>
{
    protected override InterestController CreateController(IMediator mediator)
    {
        return new InterestController(mediator);
    }

    [Test]
    public async Task CreateInterestRule_Success_ReturnsOkResult()
    {
        // Arrange
        var command = new CreateInterestRuleCommand(new InterestRuleDto
        {
            RuleId = "RULE001",
            Rate = 5.5m,
            EffectiveDate = new DateTime(2024, 1, 1)
        });

        var expectedRules = new List<InterestRuleDto>
        {
            new()
            {
                RuleId = "RULE001",
                Rate = 5.5m,
                EffectiveDate = new DateTime(2024, 1, 1)
            }
        };

        _mediatorMock.Setup(m => m.Send(command, default))
            .ReturnsAsync(new CreateInterestRuleResult(expectedRules));

        // Act
        var result = await _controller.CreateInterestRule(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var response = okResult.Value as IEnumerable<InterestRuleDto>;
        Assert.That(response.First().RuleId, Is.EqualTo("RULE001"));
    }

    [Test]
    public async Task GetInterestRules_Success_ReturnsOkResult()
    {
        // Arrange
        var fromDate = new DateTime(2024, 1, 1);
        var toDate = new DateTime(2024, 12, 31);
        var query = new GetInterestRuleByDateRangeQuery(fromDate, toDate);

        var expectedRules = new List<InterestRuleDto>
        {
            new()
            {
                RuleId = "RULE001",
                Rate = 5.5m,
                EffectiveDate = new DateTime(2024, 1, 1)
            }
        };

        _mediatorMock.Setup(m => m.Send(query, default))
            .ReturnsAsync(new GetInterestRuleByDateRangeResult(expectedRules));

        // Act
        var result = await _controller.GetInterestRules(fromDate, toDate);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        var response = okResult.Value as GetInterestRuleByDateRangeResult;
        Assert.That(response.InterestRule.First().RuleId, Is.EqualTo("RULE001"));
    }

    [Test]
    public void CreateInterestRule_WhenExceptionOccurs_ThrowsException()
    {
        // Arrange
        var command = new CreateInterestRuleCommand(new InterestRuleDto());
        var expectedError = "Invalid rule data";

        _mediatorMock.Setup(m => m.Send(command, default))
            .ThrowsAsync(new Exception(expectedError));

        // Act & Assert
        var exception = Assert.ThrowsAsync<Exception>(async () =>
            await _controller.CreateInterestRule(command));

        Assert.That(exception.Message, Is.EqualTo(expectedError));
    }

    [Test]
    public void GetInterestRules_WhenExceptionOccurs_ThrowsException()
    {
        // Arrange
        var fromDate = new DateTime(2024, 1, 1);
        var toDate = new DateTime(2024, 12, 31);
        var expectedError = "Invalid date range";

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetInterestRuleByDateRangeQuery>(), default))
            .ThrowsAsync(new Exception(expectedError));

        // Act & Assert
        var exception = Assert.ThrowsAsync<Exception>(async () =>
            await _controller.GetInterestRules(fromDate, toDate));

        Assert.That(exception.Message, Is.EqualTo(expectedError));
    }
}