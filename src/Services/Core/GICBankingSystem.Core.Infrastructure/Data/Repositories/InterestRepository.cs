using GICBankingSystem.Core.Application.Interfaces.Repository;
using GICBankingSystem.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace GICBankingSystem.Core.Infrastructure.Data.Repositories
{
    public class InterestRepository : IInterestRepository
    {
        private readonly ApplicationDbContext _context;

        public InterestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InterestEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.InterestRule
                .Where(i => i.EffectiveDate >= startDate && i.EffectiveDate <= endDate)
                .OrderBy(i => i.EffectiveDate)
                .ToListAsync();
        }

        public async Task AddTransactionAsync(InterestEntity interest)
        {
            await _context.InterestRule.AddAsync(interest);
        }

        public async Task<InterestEntity> GetByRuleIdAsync(string ruleId)
        {
            return await _context.InterestRule
           .FirstOrDefaultAsync(a => a.RuleId == ruleId);
        }

        public async Task<IEnumerable<InterestEntity>> GetAllAsync()
        {
            return await _context.InterestRule
                .OrderBy(i => i.EffectiveDate)
                .ToListAsync();
        }
    }
}
