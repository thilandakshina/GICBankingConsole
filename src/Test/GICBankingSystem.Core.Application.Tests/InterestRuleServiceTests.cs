using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Application.Interfaces.Repository;
using GICBankingSystem.Core.Application.Services;
using GICBankingSystem.Core.Application.Tests.Base;
using GICBankingSystem.Core.Domain.Exceptions;
using GICBankingSystem.Core.Domain.Models;
using Moq;

namespace GICBankingSystem.Core.Application.Tests;

[TestFixture]
public class InterestRuleServiceTests : ServiceTestBase<InterestRuleService>
{
    private Mock<IInterestRepository> _interestRepositoryMock;

    public override void Setup()
    {
        base.Setup();
        _interestRepositoryMock = new Mock<IInterestRepository>();
        UnitOfWorkMock.Setup(u => u.InterestRepository).Returns(_interestRepositoryMock.Object);

        SetupMapperMocks();
    }

    protected override InterestRuleService CreateService()
    {
        return new InterestRuleService(UnitOfWorkMock.Object, MapperMock.Object);
    }

    private void SetupMapperMocks()
    {
        MapperMock.Setup(m => m.Map<IEnumerable<InterestRuleDto>>(It.IsAny<IEnumerable<InterestEntity>>()))
            .Returns((IEnumerable<InterestEntity> entities) => entities.Select(e => new InterestRuleDto
            {
                RuleId = e.RuleId,
                EffectiveDate = e.EffectiveDate,
                Rate = e.Rate
            }));
    }

    [Test]
    public async Task CreateInterestRule_ValidRule_ShouldCreateAndReturnAllRules()
    {
        // Arrange
        var ruleDto = new InterestRuleDto
        {
            RuleId = "RULE001",
            EffectiveDate = DateTime.Now,
            Rate = 3.5m
        };

        var existingRules = new List<InterestEntity>
        {
            CreateTestInterestEntity("RULE002", DateTime.Now, 4.0m)
        };

        _interestRepositoryMock.Setup(r => r.GetByRuleIdAsync(ruleDto.RuleId))
            .ReturnsAsync((InterestEntity)null);

        _interestRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(existingRules);

        // Act
        var result = await Service.CreateInterestRule(ruleDto, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        _interestRepositoryMock.Verify(r => r.AddTransactionAsync(It.IsAny<InterestEntity>()), Times.Once);
        UnitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CreateInterestRule_DuplicateRuleId_ShouldThrowDomainException()
    {
        // Arrange
        var ruleDto = new InterestRuleDto
        {
            RuleId = "RULE001",
            EffectiveDate = DateTime.Now,
            Rate = 3.5m
        };

        _interestRepositoryMock.Setup(r => r.GetByRuleIdAsync(ruleDto.RuleId))
            .ReturnsAsync(new InterestEntity());

        // Act & Assert
        var ex = Assert.ThrowsAsync<DomainException>(async () =>
            await Service.CreateInterestRule(ruleDto, CancellationToken.None));
        Assert.That(ex.Message, Does.Contain("Rule Exist"));
    }

    [Test]
    public async Task GetAllInterestRules_WithExistingRules_ShouldReturnOrderedRules()
    {
        // Arrange
        var rules = new List<InterestEntity>
        {
            CreateTestInterestEntity("RULE001", DateTime.Now.AddDays(1), 3.5m),
            CreateTestInterestEntity("RULE002", DateTime.Now, 4.0m)
        };

        _interestRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(rules);

        // Act
        var result = await Service.GetAllInterestRules();

        // Assert
        Assert.That(result, Is.Not.Null);
        var resultList = result.ToList();
        Assert.That(resultList.Count, Is.EqualTo(2));
        Assert.That(resultList.First().RuleId, Is.EqualTo("RULE002")); // Earlier date should be first
    }

    [Test]
    public async Task GetInterestRulesByDateRange_WithValidRange_ShouldReturnMatchingRules()
    {
        // Arrange
        var fromDate = DateTime.Now;
        var toDate = fromDate.AddDays(30);
        var rules = new List<InterestEntity>
        {
            CreateTestInterestEntity("RULE001", fromDate.AddDays(1), 3.5m),
            CreateTestInterestEntity("RULE002", fromDate.AddDays(15), 4.0m)
        };

        _interestRepositoryMock.Setup(r => r.GetByDateRangeAsync(fromDate, toDate))
            .ReturnsAsync(rules);

        // Act
        var result = await Service.GetInterestRulesByDateRange(fromDate, toDate);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    private InterestEntity CreateTestInterestEntity(string ruleId, DateTime effectiveDate, decimal rate)
    {
        var entity = new InterestEntity();
        entity.RuleId = ruleId;
        entity.EffectiveDate = effectiveDate;
        entity.Rate = rate;
        return entity;
    }
}