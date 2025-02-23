using AutoMapper;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Application.Interfaces.Repository;
using GICBankingSystem.Core.Application.Interfaces.Services;
using GICBankingSystem.Core.Domain.Exceptions;
using GICBankingSystem.Core.Domain.Models;

namespace GICBankingSystem.Core.Application.Services;

public class InterestRuleService : IInterestRuleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InterestRuleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InterestRuleDto>> CreateInterestRule(InterestRuleDto ruleDto, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await ValidateInterestRule(ruleDto.RuleId);

            var interest = new InterestEntity();
            //interest.Add(ruleDto.RuleId, ruleDto.EffectiveDate, ruleDto.Rate);

            await _unitOfWork.InterestRepository.AddTransactionAsync(interest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();

            return await GetAllInterestRules();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<IEnumerable<InterestRuleDto>> GetAllInterestRules()
    {
        var allRates = await _unitOfWork.InterestRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<InterestRuleDto>>(
            allRates.OrderBy(t => t.EffectiveDate)
        );
    }

    public async Task<IEnumerable<InterestRuleDto>> GetInterestRulesByDateRange(DateTime fromDate, DateTime toDate)
    {
        var interestRules = await _unitOfWork.InterestRepository.GetByDateRangeAsync(fromDate, toDate);
        return _mapper.Map<IEnumerable<InterestRuleDto>>(interestRules);
    }

    public async Task ValidateInterestRule(string ruleId)
    {
        var ruleInfo = await _unitOfWork.InterestRepository.GetByRuleIdAsync(ruleId);
        if (ruleInfo != null)
        {
            throw new DomainException($"Rule Exist - {ruleId}");
        }
    }
}
